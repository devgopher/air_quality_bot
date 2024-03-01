using Botticelli.Client.Analytics;
using Botticelli.Framework.Commands.Validators;
using Botticelli.Shared.API.Client.Requests;
using Botticelli.Shared.ValueObjects;
using Microsoft.Extensions.Options;
using WeatherQuality.Domain.Response;
using WeatherQuality.Infrastructure;
using WeatherQuality.Integration;
using WeatherQuality.Integration.Interfaces;
using WeatherQuality.Telegram.Settings;

namespace WeatherQuality.Telegram.Commands.Processors;

public class DetailsProcessor : GenericAirQualityProcessor<DetailsCommand>
{
    public DetailsProcessor(ILogger<DetailsProcessor> logger,
        IOptionsSnapshot<WeatherQualitySettings> settings,
        ICommandValidator<DetailsCommand> validator,
        MetricsProcessor metricsProcessor,
        IAirQualityIntegration airQualityIntegration,
        GeoCacheExplorer geoCacheExplorer,
        IServiceProvider sp, ILocationService locationService) : base(logger, settings, validator, metricsProcessor,
        airQualityIntegration, geoCacheExplorer, sp, locationService)
    {
    }

    protected override Task InnerProcessContact(Message message, string args, CancellationToken token) =>
        throw new NotImplementedException();

    protected override Task InnerProcessPoll(Message message, string args, CancellationToken token) =>
        throw new NotImplementedException();

    protected override Task InnerProcessLocation(Message message, string args, CancellationToken token) =>
        throw new NotImplementedException();

    protected override async Task InnerProcess(Message message, string args, CancellationToken token)
    {
        var location = GetLocation(message);
        var address = await LocationService.GetFullAddress(location.Latitude, location.Longitude);
        var elements = typeof(Current).GetJsonPropertyNames().Where(x => x != "time" && x != "interval").ToList();
        var systemResponse = await ProcessCache(token, elements, location);
        var respMessage = CreateResponseMessage(message, "\ud83c\udf0f Main air quality indicators",
            $"\n\n\ud83d\uddfa Address: {address}" +
            $"\n\n{Settings.Value?.Criteria?.SingleOrDefault(c => c.Name == "carbon_monoxide"
                                                                  && c.LowBorder <= systemResponse.Current?.CarbonMonoxide
                                                                  && c.HiBorder > systemResponse.Current?.CarbonMonoxide)
                ?.Text} Carbon monoxide (CO):  {systemResponse.Current?.CarbonMonoxide} μg/m³ \n" +
            $"{Settings.Value?.Criteria?.SingleOrDefault(c => c.Name == "nitrogen_dioxide"
                                                              && c.LowBorder <= systemResponse.Current?.NitrogenDioxide
                                                              && c.HiBorder > systemResponse.Current?.NitrogenDioxide)
                ?.Text} Nitrogen dioxide (NO\u2082): {systemResponse.Current?.NitrogenDioxide} μg/m³ \n" +
            $"{Settings.Value?.Criteria?.SingleOrDefault(c => c.Name == "sulphur_dioxide"
                                                              && c.LowBorder <= systemResponse.Current?.SulphurDioxide
                                                              && c.HiBorder > systemResponse.Current?.SulphurDioxide)
                ?.Text} Sulphur dioxide: {systemResponse.Current?.SulphurDioxide} μg/m³ \n" +
            $"{Settings.Value?.Criteria?.SingleOrDefault(c => c.Name == "pm10"
                                                              && c.LowBorder <= systemResponse.Current?.Pm10
                                                              && c.HiBorder > systemResponse.Current?.Pm10)
                ?.Text} Dust (PM10): {systemResponse.Current?.Pm10} μg/m³ \n" +
            $"{Settings.Value?.Criteria?.SingleOrDefault(c => c.Name == "pm25"
                                                              && c.LowBorder <= systemResponse.Current?.Pm25
                                                              && c.HiBorder > systemResponse.Current?.Pm25)
                ?.Text} Dust (PM25): {systemResponse.Current?.Pm25} μg/m³");

        await _bot.SendMessageAsync(new SendMessageRequest(message.Uid)
        {
            Message = respMessage
        }, Options, token);
    }
}