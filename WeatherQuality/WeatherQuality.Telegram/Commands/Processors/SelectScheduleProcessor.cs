using Botticelli.Client.Analytics;
using Botticelli.Framework.Commands.Processors;
using Botticelli.Framework.Commands.Validators;
using Botticelli.Framework.SendOptions;
using Botticelli.Shared.API.Client.Requests;
using Botticelli.Shared.ValueObjects;
using Telegram.Bot.Types.ReplyMarkups;

namespace WeatherQuality.Telegram.Commands.Processors;

public class SelectScheduleProcessor : CommandProcessor<SelectScheduleCommand>
{
    private readonly SendOptionsBuilder<ReplyMarkupBase> _options;

    public SelectScheduleProcessor(ILogger<SelectScheduleProcessor> logger,
        ICommandValidator<SelectScheduleCommand> validator,
        MetricsProcessor metricsProcessor)
        : base(logger, validator, metricsProcessor)
    {
        _options = SendOptionsBuilder<ReplyMarkupBase>.CreateBuilder(new ReplyKeyboardMarkup(new[]
        {
            Enumerable.Range(0, 6).Select(
                x => new KeyboardButton($"/minute {x:D2}")
                {
                    RequestLocation = false
                }
            ),
            Enumerable.Range(6, 6).Select(
                x => new KeyboardButton($"/minute {x:D2}")
                {
                    RequestLocation = false
                }
            ),
            Enumerable.Range(12, 6).Select(
                x => new KeyboardButton($"/minute {x:D2}")
                {
                    RequestLocation = false
                }
            ),
            Enumerable.Range(18, 6).Select(
                x => new KeyboardButton($"/minute {x:D2}")
                {
                    RequestLocation = false
                }
            ),
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
        var selectHourMessageRequest = new SendMessageRequest(Guid.NewGuid().ToString())
        {
            Message = new Message
            {
                Uid = Guid.NewGuid().ToString(),
                ChatIds = message.ChatIds,
                Body = "Select hour..."
            }
        };

        await Bot.SendMessageAsync(selectHourMessageRequest, _options, token);
    }
}