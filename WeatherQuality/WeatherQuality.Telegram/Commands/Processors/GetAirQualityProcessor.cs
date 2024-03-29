using Botticelli.Bot.Interfaces.Bus.Handlers;
using Botticelli.Bot.Interfaces.Client;
using Botticelli.Client.Analytics;
using Botticelli.Framework.Commands.Processors;
using Botticelli.Framework.Commands.Validators;
using Botticelli.Shared.API.Client.Requests;
using Botticelli.Shared.API.Client.Responses;
using Botticelli.Shared.ValueObjects;
using WeatherQuality.Telegram.Services;

namespace WeatherQuality.Telegram.Commands.Processors;

public class GetAirQualityProcessor : CommandProcessor<GetAirQualityCommand>
{
    private readonly IAqiDataProcessor _aqiDataProcessor;
    private readonly IBusClient _busClient;

    public GetAirQualityProcessor(ILogger<GetAirQualityProcessor> logger,
        ICommandValidator<GetAirQualityCommand> validator,
        MetricsProcessor metricsProcessor,
        IAqiDataProcessor aqiDataProcessor,
        IBusClient busClient) : base(logger,
        validator, metricsProcessor)
    {
        _aqiDataProcessor = aqiDataProcessor;
        _busClient = busClient;
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
        await _aqiDataProcessor.AqiProcess(message, token);
        await _busClient.SendAndGetResponse(new SendMessageRequest(Guid.NewGuid().ToString()), token);
    }
}