using Botticelli.Client.Analytics;
using Botticelli.Framework.Commands.Processors;
using Botticelli.Framework.Commands.Validators;
using Botticelli.Framework.SendOptions;
using Botticelli.Shared.API.Client.Requests;
using Botticelli.Shared.ValueObjects;
using Telegram.Bot.Types.ReplyMarkups;

namespace WeatherQuality.Telegram.Commands.Processors;

public class StartCommandProcessor : CommandProcessor<StartCommand>
{
    private readonly SendOptionsBuilder<ReplyMarkupBase> _options;

    public StartCommandProcessor(ILogger<StartCommandProcessor> logger,
        ICommandValidator<StartCommand> validator,
        MetricsProcessor metricsProcessor)
        : base(logger, validator, metricsProcessor)
    {
        _options = SendOptionsBuilder<ReplyMarkupBase>.CreateBuilder(new ReplyKeyboardMarkup(new[]
        {
            new[]
            {
                new KeyboardButton("/SetLocation")
                {
                    RequestLocation = true
                }
            }
        })
        {
            ResizeKeyboard = true
        });
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

        await Bot.SendMessageAsync(greetingMessageRequest, _options, token);
    }
}