using Hangfire;
using Microsoft.Extensions.Options;
using WeatherQuality.Infrastructure;
using WeatherQuality.Telegram.Settings;

namespace WeatherQuality.Telegram.Jobs;

[DisableConcurrentExecution(timeoutInSeconds:60)]
public class ClearCacheJob
{
    private readonly WeatherQualityContext _context;
    private readonly IOptionsSnapshot<WeatherQualitySettings> _settings;
    private readonly ILogger<ClearCacheJob> _logger;

    public ClearCacheJob(WeatherQualityContext context, ILogger<ClearCacheJob> logger,
        IOptionsSnapshot<WeatherQualitySettings> settings)
    {
        _context = context;
        _logger = logger;
        _settings = settings;
    }

    public async Task Execute(IJobCancellationToken token)
    {
        _logger.LogInformation($"Cache cleaning started...");

        try
        {
            var toRemove = _context.GeoCacheModels
                .Where(x => x.Timestamp < DateTime.UtcNow
                    .AddHours(_settings.Value.CacheCleaningAge))
                .Take(_settings.Value.CacheCleaningBatchSize);
            
            _logger.LogInformation($"Trying to remove {toRemove.Count()} records...");
            
            _context.GeoCacheModels.RemoveRange(toRemove);

            await _context.SaveChangesAsync(token.ShutdownToken);
        }
        catch (Exception ex)
        {
            _logger.LogError(ex, "Cache cleaning error: {msg}", ex.Message);
        }

        _logger.LogInformation($"Cache cleaning end...");
    }
    
}