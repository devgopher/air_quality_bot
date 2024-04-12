using Botticelli.Client.Analytics;
using Botticelli.Framework.Commands.Processors;
using Botticelli.Framework.Commands.Validators;
using Botticelli.Framework.SendOptions;
using Botticelli.Shared.API.Client.Requests;
using Botticelli.Shared.ValueObjects;
using Telegram.Bot.Types.ReplyMarkups;

namespace WeatherQuality.Telegram.Commands.Processors;

public class SelectScheduleHourProcessor : CommandProcessor<HourCommand>
{
    public SelectScheduleHourProcessor(ILogger<SelectScheduleHourProcessor> logger,
        ICommandValidator<HourCommand> validator,
        MetricsProcessor metricsProcessor)
        : base(logger, validator, metricsProcessor)
    {
    }

    protected override Task InnerProcessContact(Message message, string argsString, CancellationToken token)
    {
        return Task.CompletedTask;
    }

    protected override Task InnerProcessPoll(Message message, string argsString, CancellationToken token)
    {
        return Task.CompletedTask;
    }

    protected override Task InnerProcessLocation(Message message, string argsString, CancellationToken token)
    {
        return Task.CompletedTask;
    }

    private static readonly int[] MinutesArray = new[] { 0, 15, 30, 45 };

    protected override async Task InnerProcess(Message message, string args, CancellationToken token)
    {
        var options = SendOptionsBuilder<ReplyMarkupBase>.CreateBuilder(new ReplyKeyboardMarkup(new[]
        { 
            MinutesArray.Select(
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