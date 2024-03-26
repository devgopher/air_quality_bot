using Botticelli.Client.Analytics;
using Botticelli.Framework.Commands.Processors;
using Botticelli.Framework.Commands.Validators;
using Botticelli.Shared.ValueObjects;
using Hangfire;
using Hangfire.Storage;

namespace WeatherQuality.Telegram.Commands.Processors;

public class CleanScheduleProcessor : CommandProcessor<CleanScheduleCommand>
 {
    public CleanScheduleProcessor(ILogger<CleanScheduleProcessor> logger, ICommandValidator<CleanScheduleCommand> validator, MetricsProcessor metricsProcessor) : base(logger, validator, metricsProcessor) 
    {}

    protected override async Task InnerProcessContact(Message message, string args, CancellationToken token) => throw new NotImplementedException();

    protected override async Task InnerProcessPoll(Message message, string args, CancellationToken token) => throw new NotImplementedException();

    protected override async Task InnerProcessLocation(Message message, string args, CancellationToken token) => throw new NotImplementedException();

    protected override async Task InnerProcess(Message message, string args, CancellationToken token)
    {
      
        foreach (var chatId in message.ChatIds)
        {
            var jobs = JobStorage.Current.GetConnection().GetRecurringJobs().Where(j => j.Id.StartsWith($"SendAqiJob_{chatId}"));

            foreach (var job in jobs) 
                RecurringJob.RemoveIfExists(job.Id);
        }
    }
}