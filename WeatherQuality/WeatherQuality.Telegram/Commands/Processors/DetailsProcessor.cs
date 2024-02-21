using Botticelli.Client.Analytics;
using Botticelli.Framework.Commands.Processors;
using Botticelli.Framework.Commands.Validators;
using Botticelli.Shared.API.Client.Requests;
using Botticelli.Shared.ValueObjects;
using Microsoft.EntityFrameworkCore;
using Newtonsoft.Json;
using WeatherQuality.Domain.Request;
using WeatherQuality.Domain.Response;
using WeatherQuality.Integration;
using WeatherQuality.Telegram.Database;
using WeatherQuality.Telegram.Database.Models;

namespace WeatherQuality.Telegram.Commands.Processors;

public class DetailsProcessor : CommandProcessor<DetailsCommand>
{
    private readonly IIntegration _integration;
    private readonly WeatherQualityContext _context;

    public DetailsProcessor(ILogger<DetailsProcessor> logger,
                            ICommandValidator<DetailsCommand> validator,
                            MetricsProcessor metricsProcessor,
                            IIntegration integration,
                            WeatherQualityContext context) : base(logger, validator, metricsProcessor)
    {
        _integration = integration;
        _context = context;
    }

    protected override Task InnerProcessContact(Message message, string args, CancellationToken token) => throw new NotImplementedException();

    protected override Task InnerProcessPoll(Message message, string args, CancellationToken token) => throw new NotImplementedException();

    protected override Task InnerProcessLocation(Message message, string args, CancellationToken token) => throw new NotImplementedException();

    protected override async Task InnerProcess(Message message, string args, CancellationToken token)
    {
        Message? respMessage;

        var cached = await _context.AirQualityCacheDetailsModels
                                   .FirstOrDefaultAsync(c => message.ChatIds.Contains(c.ChatId) &&
                                                             DateTime.UtcNow - c.Timestamp < TimeSpan.FromHours(1),
                                                        token);

        if (cached != null)
        {
            respMessage = JsonConvert.DeserializeObject<Message>(cached.SerializedResponse,
                                                                 new JsonSerializerSettings
                                                                 {
                                                                     Error = (sender, error) => error.ErrorContext.Handled = true
                                                                 });
        }
        else
        {
            var response = await GetQuality(message);
            respMessage = CreateMessage(message, response);

            foreach (var cachedMessage in message.ChatIds.Select(chatId
                                                                         => new AirQualityCacheDetailsModel()
                                                                         {
                                                                             Id = Guid.NewGuid(),
                                                                             ChatId = chatId,
                                                                             Timestamp = DateTime.UtcNow,
                                                                             SerializedResponse = JsonConvert.SerializeObject(respMessage),
                                                                             Radius = 2.0
                                                                         }))
            {
                await _context.AirQualityCacheDetailsModels.AddAsync(cachedMessage, token);
                await _context.SaveChangesAsync(token);
            }
        }

        await _bot.SendMessageAsync(new SendMessageRequest(message.Uid)
                                    {
                                        Message = respMessage
                                    },
                                    token);
    }

    private static Message? CreateMessage(Message message, Response response)
    {
        var respMessage = new Message
        {
            Uid = message.Uid,
            ChatIds = message.ChatIds,
            Subject = "Main air quality indicators. \n",
            Body = $"\n\nCarbon monoxide: {response.Current?.CarbonMonoxide} μg/m^3 \n" +
                   $"Nitrogen dioxide: {response.Current?.NitrogenDioxide} μg/m^3 \n" +
                   $"Sulphur dioxide: {response.Current?.SulphurDioxide} μg/m^3 \n" +
                   $"Dust: {response.Current?.Dust} μg/m^3"
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
            Current = "carbon_monoxide,nitrogen_dioxide,sulphur_dioxide,ozone,aerosol_optical_depth,dust"
        });

        return response;
    }
}