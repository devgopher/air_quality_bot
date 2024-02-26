using Botticelli.Client.Analytics;
using Botticelli.Framework.Commands.Validators;
using Botticelli.Framework.SendOptions;
using Botticelli.Shared.API.Client.Requests;
using Botticelli.Shared.Constants;
using Botticelli.Shared.ValueObjects;
using SixLabors.ImageSharp;
using Telegram.Bot.Types.ReplyMarkups;
using WeatherQuality.Domain.Response;
using WeatherQuality.Infrastructure;
using WeatherQuality.Integration;

namespace WeatherQuality.Telegram.Commands.Processors;

public class GetAirQualityProcessor : GenericAirQualityProcessor<GetAirQualityCommand>
{
    private readonly SendOptionsBuilder<ReplyMarkupBase> _options;

    
    public GetAirQualityProcessor(ILogger<GetAirQualityProcessor> logger, ICommandValidator<GetAirQualityCommand> validator,
        MetricsProcessor metricsProcessor, IIntegration integration,
        GeoCacheExplorer geoCacheExplorer,
        WeatherQualityContext context) : base(logger, validator, metricsProcessor, integration, geoCacheExplorer, context)
    {
        _options = SendOptionsBuilder<ReplyMarkupBase>.CreateBuilder(new ReplyKeyboardMarkup(new[]
                                                                     {
                                                                         new[]
                                                                         {
                                                                             new KeyboardButton("/Details")
                                                                             {
                                                                                 RequestLocation = false
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
        var location = GetLocation(message);
        var elements = new List<string>()
        {
            "european_aqi"
        };

        var systemResponse = await ProcessCache(token, elements, location);

        var generatedImage = systemResponse.Current?.EuropeanAqi switch
        {
            < 50 => GenerateImage(systemResponse, @"Images\no_pollution.jpg"),
            >= 50 and < 100 => GenerateImage(systemResponse, @"Images\middle_air_pollution.jpg"),
            >= 100 => GenerateImage(systemResponse, @"Images\extreme_pollution.jpg"),
            _ => null
        };

        var respMessage = CreateResponseMessage(message,
            "AQI in: ",
            !string.IsNullOrWhiteSpace(systemResponse.Error)
                ? systemResponse.Error
                : $"{systemResponse?.Latitude}, {systemResponse?.Longitude}", generatedImage);

        await _bot.SendMessageAsync(new SendMessageRequest(message.Uid)
        {
            Message = respMessage
        }, _options, token);
    }

    private static byte[]? GenerateImage(Response response, string path)
    {
        var image = ImageUtils.PlaceText(path,
                                         $"AQI: {response.Current?.EuropeanAqi}", 200f,
                                         Color.Red, 200, 350);
        return image;
    }
}