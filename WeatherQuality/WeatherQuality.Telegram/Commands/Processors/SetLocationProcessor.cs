using Botticelli.Client.Analytics;
using Botticelli.Framework.Commands.Processors;
using Botticelli.Framework.Commands.Validators;
using Botticelli.Shared.ValueObjects;

namespace WeatherQuality.Telegram.Commands.Processors;

public class SetLocationProcessor : CommandProcessor<SetLocationCommand>
{
    public SetLocationProcessor(ILogger logger, ICommandValidator<SetLocationCommand> validator, MetricsProcessor metricsProcessor) : base(logger, validator, metricsProcessor)
    {
    }

    protected override Task InnerProcessContact(Message message, string args, CancellationToken token) => throw new NotImplementedException();

    protected override Task InnerProcessPoll(Message message, string args, CancellationToken token) => throw new NotImplementedException();

    protected override Task InnerProcessLocation(Message message, string args, CancellationToken token) => throw new NotImplementedException();

    protected override Task InnerProcess(Message message, string args, CancellationToken token) => throw new NotImplementedException();
}