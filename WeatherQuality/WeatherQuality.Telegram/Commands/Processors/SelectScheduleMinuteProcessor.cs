using Botticelli.Client.Analytics;
using Botticelli.Framework.Commands.Processors;
using Botticelli.Framework.Commands.Validators;
using Botticelli.Framework.SendOptions;
using Botticelli.Shared.API.Client.Requests;
using Botticelli.Shared.ValueObjects;
using Telegram.Bot.Types.ReplyMarkups;

namespace WeatherQuality.Telegram.Commands.Processors;

public class SelectScheduleMinuteProcessor : CommandProcessor<Minute>
{
    public SelectScheduleMinuteProcessor(ILogger<SelectScheduleMinuteProcessor> logger,
        ICommandValidator<Minute> validator,
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
        var options = SendOptionsBuilder<ReplyMarkupBase>.CreateBuilder(new ReplyKeyboardMarkup(new[]
        {
            new[] { 0, 15, 30, 45 }.Select(
                x => new KeyboardButton($"/Schedule {args}:{x:D2}")
                {
                    RequestLocation = false
                }
            )
        })
        {
            ResizeKeyboard = true
        });

        var selectHourMessageRequest = new SendMessageRequest(Guid.NewGuid().ToString())
        {
            Message = new Message
            {
                Uid = Guid.NewGuid().ToString(),
                ChatIds = message.ChatIds,
                Body = "Select minute..."
            }
        };

        await Bot.SendMessageAsync(selectHourMessageRequest, options, token);
    }
}