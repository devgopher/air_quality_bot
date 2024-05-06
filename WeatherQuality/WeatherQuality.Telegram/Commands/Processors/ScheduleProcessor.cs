using Botticelli.Client.Analytics;
using Botticelli.Framework.Commands.Processors;
using Botticelli.Framework.Commands.Validators;
using Botticelli.Shared.API.Client.Requests;
using Botticelli.Shared.ValueObjects;
using Hangfire;
using WeatherQuality.Telegram.Commands.ReplyOptions;
using WeatherQuality.Telegram.Jobs;
using WeatherQuality.Telegram.Services;

namespace WeatherQuality.Telegram.Commands.Processors;

public class ScheduleProcessor : CommandProcessor<ScheduleCommand>
{
    private readonly IGetLocationService _locationService;
    private readonly IRecurringJobManager _recurringJobManager;
    
    public ScheduleProcessor(ILogger<ScheduleProcessor> logger,
        ICommandValidator<ScheduleCommand> validator,
        MetricsProcessor metricsProcessor,
        IGetLocationService locationService,
        IRecurringJobManager recurringJobManager) : base(logger,
                                                         validator, metricsProcessor)
    {
        _locationService = locationService;
        _recurringJobManager = recurringJobManager;
    }

    protected override Task InnerProcessContact(Message message, string args, CancellationToken token)
    {
        throw new NotImplementedException();
    }

    protected override Task InnerProcessPoll(Message message, string args, CancellationToken token)
    {
        throw new NotImplementedException();
    }

    protected override Task InnerProcessLocation(Message message, string args, CancellationToken token)
    {
        throw new NotImplementedException();
    }

    protected override async Task InnerProcess(Message message, string args, CancellationToken token)
    {
        var hhMm = args.Split(':').Select(int.Parse).ToArray();

        foreach (var chatId in message.ChatIds)
        {
            var location = _locationService.GetLocation(message);

            RecurringJob.AddOrUpdate<SendAqiJob>(recurringJobId: $"SendAqiJob_{chatId}_{args}",
                                                 methodCall: job => job.Execute(message, JobCancellationToken.Null),
                                                 cronExpression: () => Cron.Daily(hhMm.First(),hhMm.Skip(1).First()), 
                                                 timeZone: await _locationService.GetTimeZone(location!),
                                                 queue: "default");

        }

        var selectHourMessageRequest = new SendMessageRequest(Guid.NewGuid().ToString())
        {
            Message = new Message
            {
                Uid = Guid.NewGuid().ToString(),
                ChatIds = message.ChatIds,
                Body = "Schedule submitted..."
            }
        };

        await Bot.SendMessageAsync(selectHourMessageRequest, Replies.GeneralReplyOptions, token);
    }
}