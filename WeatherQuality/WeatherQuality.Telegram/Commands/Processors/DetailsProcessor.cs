using Botticelli.Client.Analytics;
using Botticelli.Framework.Commands.Processors;
using Botticelli.Framework.Commands.Validators;
using Botticelli.Shared.API.Client.Requests;
using Botticelli.Shared.ValueObjects;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Options;
using Newtonsoft.Json;
using WeatherQuality.Domain.Request;
using WeatherQuality.Domain.Response;
using WeatherQuality.Infrastructure;
using WeatherQuality.Infrastructure.Models;
using WeatherQuality.Integration;
using WeatherQuality.Telegram.Settings;

namespace WeatherQuality.Telegram.Commands.Processors;

public class DetailsProcessor : GenericAirQualityProcessor<DetailsCommand>
{
    public DetailsProcessor(ILogger<DetailsProcessor> logger,
        IOptionsSnapshot<WeatherQualitySettings> settings,
        ICommandValidator<DetailsCommand> validator,
        MetricsProcessor metricsProcessor,
        IIntegration integration,
        GeoCacheExplorer geoCacheExplorer,
        IServiceProvider sp, ILocationService locationService) : base(logger, settings, validator, metricsProcessor,
        integration, geoCacheExplorer, sp, locationService)
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
        var elements = typeof(Current).GetJsonPropertyNames().Where(x => x != "time" && x != "interval").ToList();
        var systemResponse = await ProcessCache(token, elements, location);
        var respMessage = CreateResponseMessage(message, "Main air quality indicators. \n",
            $"\n\nCarbon monoxide: {systemResponse.Current?.CarbonMonoxide} μg/m^3 \n" +
            $"Nitrogen dioxide: {systemResponse.Current?.NitrogenDioxide} μg/m^3 \n" +
            $"Sulphur dioxide: {systemResponse.Current?.SulphurDioxide} μg/m^3 \n" +
            $"Dust: {systemResponse.Current?.Dust} μg/m^3");

        await _bot.SendMessageAsync(new SendMessageRequest(message.Uid)
        {
            Message = respMessage
        }, Options, token);
    }
}