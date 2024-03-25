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

    public DetailsProcessor(ILogger<DetailsProcessor> logger,
        ICommandValidator<DetailsCommand> validator,
        MetricsProcessor metricsProcessor,
        IDetailsDataProcessor detailsDataProcessor) : base(logger,
        validator, metricsProcessor)
    {
        _detailsDataProcessor = detailsDataProcessor;
    }

    protected override Task InnerProcessContact(Message message, string args, CancellationToken token) =>
        throw new NotImplementedException();

    protected override Task InnerProcessPoll(Message message, string args, CancellationToken token) =>
        throw new NotImplementedException();

    protected override Task InnerProcessLocation(Message message, string args, CancellationToken token) =>
        throw new NotImplementedException();

    protected override async Task InnerProcess(Message message, string args, CancellationToken token) 
        => await _detailsDataProcessor.DetailsProcess(message, token);
}