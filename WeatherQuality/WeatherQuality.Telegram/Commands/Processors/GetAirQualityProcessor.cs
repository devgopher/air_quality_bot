using Botticelli.Client.Analytics;
using Botticelli.Framework.Commands.Processors;
using Botticelli.Framework.Commands.Validators;
using Botticelli.Framework.SendOptions;
using Botticelli.Shared.API.Client.Requests;
using Botticelli.Shared.Constants;
using Botticelli.Shared.ValueObjects;
using Microsoft.EntityFrameworkCore;
using Newtonsoft.Json;
using SixLabors.ImageSharp;
using Telegram.Bot.Types.ReplyMarkups;
using WeatherQuality.Domain.Request;
using WeatherQuality.Domain.Response;
using WeatherQuality.Integration;
using WeatherQuality.Telegram.Database;
using WeatherQuality.Telegram.Database.Models;

namespace WeatherQuality.Telegram.Commands.Processors;

public class GetAirQualityProcessor : CommandProcessor<GetAirQualityCommand>
{
    private readonly IIntegration _integration;
    private readonly WeatherQualityContext _context;
    private readonly SendOptionsBuilder<ReplyMarkupBase> _options;
    
    public GetAirQualityProcessor(ILogger<GetAirQualityProcessor> logger, ICommandValidator<GetAirQualityCommand> validator,
        MetricsProcessor metricsProcessor, IIntegration integration, WeatherQualityContext context) : base(logger, validator, metricsProcessor)
    {
        _integration = integration;
        _context = context;
        _options = SendOptionsBuilder<ReplyMarkupBase>.CreateBuilder(new ReplyKeyboardMarkup(new[]
                                                                     {
                                                                         new[]
                                                                         {
                                                                             new KeyboardButton("/Details")
                                                                             {
                                                                                 RequestLocation = true
                                                                             }
                                                                         }
                                                                     })
                                                                     {
                                                                         ResizeKeyboard = true
                                                                     });
    }

    protected override async Task InnerProcessContact(Message message, string args, CancellationToken token)
    {
    }

    protected override async Task InnerProcessPoll(Message message, string args, CancellationToken token)
    {
    }

    protected override async Task InnerProcessLocation(Message message, string args, CancellationToken token)
    {
    }

    protected override async Task InnerProcess(Message message, string args, CancellationToken token)
    {
        Message? respMessage;

        var cached = await _context.AirQualityCacheModels
            .FirstOrDefaultAsync(c => message.ChatIds.Contains(c.ChatId) &&
                                      DateTime.UtcNow - c.Timestamp < TimeSpan.FromHours(1), token);
        if (cached != null)
        {
            respMessage = JsonConvert.DeserializeObject<Message>(cached.SerializedResponse, new JsonSerializerSettings()
            {
                Error = (sender, error) => error.ErrorContext.Handled = true
            });
        }
        else
        {
            var response = await GetQuality(message);
            var generatedImage = response.Current?.EuropeanAqi switch
            {
                < 50 => GenerateImage(response, @"Images\no_pollution.jpg"),
                >= 50 and < 100 => GenerateImage(response, @"Images\middle_air_pollution.jpg"),
                _ => GenerateImage(response, @"Images\extreme_pollution.jpg")
            };

            respMessage = CreateMessage(message, response, generatedImage);

            foreach (var cachedMessage in message.ChatIds.Select(chatId
                                                                         => new AirQualityCacheModel
                                                                         {
                                                                             Id = Guid.NewGuid(),
                                                                             ChatId = chatId,
                                                                             Timestamp = DateTime.UtcNow,
                                                                             SerializedResponse = JsonConvert.SerializeObject(respMessage),
                                                                             Radius = 2.0
                                                                         }))
            {
                await _context.AirQualityCacheModels.AddAsync(cachedMessage, token);
                await _context.SaveChangesAsync(token);
            }
        }

        await _bot.SendMessageAsync(new SendMessageRequest(message.Uid)
        {
            Message = respMessage
        }, _options, token);
    }

    private static Message? CreateMessage(Message message, Response response, byte[] image)
    {
        var respMessage = new Message
        {
            Uid = message.Uid,
            ChatIds = message.ChatIds,
            Subject = "AQI in: ",
            Body = $"{response?.Latitude}, {response?.Longitude}",
            Attachments = new List<IAttachment>()
            {
                new BinaryAttachment(Guid.NewGuid().ToString(), "air", MediaType.Image, string.Empty, image)
            }
        };
        return respMessage;
    }

    private async Task<Response> GetQuality(Message message)
    {
        var record = _context.UserLocationModels.FirstOrDefault(l => message.ChatIds.Contains(l.ChatId));
        if (record == default)
            return new Response
            {
                IsSuccess = false,
                Error = "Please, enter your geolocation",
                Latitude = 0,
                Longitude = 0,
                GenerationtimeMs = 0,
                UtcOffsetSeconds = 0,
                Timezone = null,
                TimezoneAbbreviation = null,
                Elevation = 0,
                HourlyUnits = null,
                Hourly = null,
                Current = null
            };

        var response = await _integration.GetAirQualityAsync(new Request
        {
            Latitude = record.Latitude,
            Longitude = record.Longitude,
            Current = "european_aqi",
            Hourly = "european_aqi"
        });
        
        return response;
    }

    private static byte[] GenerateImage(Response response, string path)
    {
        var image = ImageUtils.PlaceText(path,
            $"AQI: {response.Current?.EuropeanAqi}", 200f,
            Color.Red, 200, 350);
        return image;
    }
}