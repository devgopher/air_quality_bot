using Botticelli.Bot.Interfaces.Client;
using Botticelli.Client.Analytics;
using Botticelli.Framework.Commands.Processors;
using Botticelli.Framework.Commands.Validators;
using Botticelli.Shared.API.Client.Requests;
using Botticelli.Shared.ValueObjects;
using WeatherQuality.Domain.Response;
using WeatherQuality.Integration;
using WeatherQuality.Telegram.Services;

namespace WeatherQuality.Telegram.Commands.Processors;

public class DetailsProcessor : CommandProcessor<DetailsCommand>
{
    private readonly IDetailsDataProcessor _detailsDataProcessor;
    private readonly IBusClient _busClient;

    public DetailsProcessor(ILogger<DetailsProcessor> logger,
        ICommandValidator<DetailsCommand> validator,
        MetricsProcessor metricsProcessor,
        IDetailsDataProcessor detailsDataProcessor, IBusClient busClient) : base(logger,
        validator, metricsProcessor)
    {
        _detailsDataProcessor = detailsDataProcessor;
        _busClient = busClient;
    }

    protected override Task InnerProcessContact(Message message, string args, CancellationToken token) =>
        throw new NotImplementedException();

    protected override Task InnerProcessPoll(Message message, string args, CancellationToken token) =>
        throw new NotImplementedException();

    protected override Task InnerProcessLocation(Message message, string args, CancellationToken token) {}

    protected override async Task InnerProcess(Message message, string args, CancellationToken token)
    {
        await _detailsDataProcessor.DetailsProcess(message, token);
        await _busClient.SendAndGetResponse(new SendMessageRequest(Guid.NewGuid().ToString()), token);
    }
}