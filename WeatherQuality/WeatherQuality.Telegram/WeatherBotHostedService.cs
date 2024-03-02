using Botticelli.Framework.Telegram;
using Botticelli.Interfaces;
using Hangfire;
using Microsoft.Extensions.Options;
using WeatherQuality.Telegram.Jobs;
using WeatherQuality.Telegram.Settings;

namespace WeatherQuality.Telegram;

/// <summary>
///     This hosted service intended for sending messages according to a schedule
/// </summary>
public class WeatherBotHostedService : IHostedService
{
    private readonly IOptionsMonitor<WeatherQualitySettings> _settings;
    private readonly IBot<TelegramBot> _telegramBot;

    public WeatherBotHostedService(IBot<TelegramBot> telegramBot, IOptionsMonitor<WeatherQualitySettings> settings)
    {
        _telegramBot = telegramBot;
        _settings = settings;
    }

    public Task StartAsync(CancellationToken token)
    {
        Console.WriteLine("Start sending messages...");
        
        RecurringJob.AddOrUpdate<ClearCacheJob>("CleanCacheJob", 
            job => job.Execute(JobCancellationToken.Null),
            Cron.Daily);

        return Task.CompletedTask;
    }

    public Task StopAsync(CancellationToken cancellationToken)
    {
        Console.WriteLine("Stop sending messages...");

        RecurringJob.RemoveIfExists("CleanCacheJob");
        
        return Task.CompletedTask;
    }
}