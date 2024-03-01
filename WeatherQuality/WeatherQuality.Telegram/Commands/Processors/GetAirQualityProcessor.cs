using Botticelli.Client.Analytics;
using Botticelli.Framework.Commands.Validators;
using Botticelli.Shared.API.Client.Requests;
using Botticelli.Shared.ValueObjects;
using Microsoft.Extensions.Options;
using SixLabors.ImageSharp;
using WeatherQuality.Domain.Response;
using WeatherQuality.Infrastructure;
using WeatherQuality.Integration;
using WeatherQuality.Integration.Interfaces;
using WeatherQuality.Telegram.Settings;

namespace WeatherQuality.Telegram.Commands.Processors;

public class GetAirQualityProcessor : GenericAirQualityProcessor<GetAirQualityCommand>
{
    public GetAirQualityProcessor(ILogger<GetAirQualityProcessor> logger,
        IOptionsSnapshot<WeatherQualitySettings> settings,
        ICommandValidator<GetAirQualityCommand> validator,
        MetricsProcessor metricsProcessor, IAirQualityIntegration airQualityIntegration,
        GeoCacheExplorer geoCacheExplorer,
        IServiceProvider sp, ILocationService locationService) : base(logger, settings, validator, metricsProcessor,
        airQualityIntegration, geoCacheExplorer, sp, locationService)
    {
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

        var aqi = systemResponse.Current?.EuropeanAqi;
        var decision = Settings.Value.Criteria?.SingleOrDefault(c => aqi >= c.LowBorder && aqi < c.HiBorder && c.Name == "european_aqi");
        if (decision == default)
            throw new InvalidDataException($"Can't get an appropriate decision for aqi {aqi}! " +
                                           $"Please, check out your configuration ('Criteria' section)!");

        var generatedImage = GenerateImage(systemResponse, decision);

        var address =
            await LocationService.GetFullAddress(location.Latitude, location.Longitude);
        
        var respMessage = CreateResponseMessage(message,
            decision.Text,
            !string.IsNullOrWhiteSpace(systemResponse.Error)
                ? systemResponse.Error
                : address, generatedImage);

        await _bot.SendMessageAsync(new SendMessageRequest(message.Uid)
        {
            Message = respMessage
        }, Options, token);
    }

    private static byte[] GenerateImage(Response response, MetricCriteria criteria)
    {
        var image = ImageUtils.PlaceText(criteria.ImagePath,
            $"AQI: {response.Current?.EuropeanAqi}", 200f,
            Color.Red, 200, 50);

        return criteria.Recommendations.Aggregate(image, (current, recommendation) 
            => ImageUtils.PlaceText(current, recommendation, 
                100f, 
                Color.Parse(criteria.Color), 
                200, 
                250 + criteria.Recommendations.IndexOf(recommendation) * 100));
    }
}