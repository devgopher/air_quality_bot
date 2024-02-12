using Botticelli.Client.Analytics;
using Botticelli.Framework.Commands.Processors;
using Botticelli.Framework.Commands.Validators;
using Botticelli.Shared.API.Client.Requests;
using Botticelli.Shared.Constants;
using Botticelli.Shared.ValueObjects;

namespace WeatherQuality.Telegram.Commands.Processors;

public class StartCommandProcessor : CommandProcessor<StartCommand>
{
    public StartCommandProcessor(ILogger<StartCommandProcessor> logger,
        ICommandValidator<StartCommand> validator,
        MetricsProcessor metricsProcessor)
        : base(logger, validator, metricsProcessor)
    {
    }

    protected override async Task InnerProcessContact(Message message, string argsString, CancellationToken token)
    {
    }

    protected override async Task InnerProcessPoll(Message message, string argsString, CancellationToken token)
    {
    }

    protected override async Task InnerProcessLocation(Message message, string argsString, CancellationToken token)
    {
    }

    protected override async Task InnerProcess(Message message, string args, CancellationToken token)
    {
        var chatId = message.ChatIds.FirstOrDefault();
        var greetingMessageRequest = new SendMessageRequest(Guid.NewGuid().ToString())
        {
            Message = new Message
            {
                Uid = Guid.NewGuid().ToString(),
                ChatIds = message.ChatIds,
                Body = "Bot started..."
            }
        };

        await _bot.SendMessageAsync(greetingMessageRequest, token);
    }
}