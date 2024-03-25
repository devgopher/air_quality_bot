using Botticelli.Client.Analytics;
using Botticelli.Framework.Commands.Processors;
using Botticelli.Framework.Commands.Validators;
using Botticelli.Shared.ValueObjects;
using Hangfire;
using WeatherQuality.Telegram.Jobs;
using WeatherQuality.Telegram.Services;

namespace WeatherQuality.Telegram.Commands.Processors;

public class ScheduleProcessor : CommandProcessor<ScheduleCommand>
{
    private readonly IGetLocationService _locationService;
    
    public ScheduleProcessor(ILogger<DetailsProcessor> logger,
        ICommandValidator<ScheduleCommand> validator,
        MetricsProcessor metricsProcessor,
        IGetLocationService locationService) : base(logger,
        validator, metricsProcessor)
    {
        _locationService = locationService;
    }

    protected override async Task InnerProcessContact(Message message, string args, CancellationToken token) => throw new NotImplementedException();

    protected override async Task InnerProcessPoll(Message message, string args, CancellationToken token) => throw new NotImplementedException();

    protected override async Task InnerProcessLocation(Message message, string args, CancellationToken token) => throw new NotImplementedException();

    protected override async Task InnerProcess(Message message, string args, CancellationToken token)
    {
        foreach (var chatId in message.ChatIds)
        {
            var location = _locationService.GetLocation(message);

            var hhMm = args.Split(':'); 
            
            RecurringJob.AddOrUpdate<SendAqiJob>(
                recurringJobId: $"SendAqiJob_{chatId}_{args}", 
               methodCall: job => job.Execute(message, JobCancellationToken.Null),
               cronExpression: () => $"{hhMm[1]} {hhMm[0]} * * *", 
               timeZone: await _locationService.GetTimeZone(location!),
               queue : "default");
        }
    }
}