using Botticelli.Shared.ValueObjects;
using Hangfire;
using WeatherQuality.Telegram.Services;

namespace WeatherQuality.Telegram.Jobs;

[DisableConcurrentExecution(timeoutInSeconds:300)]
public class SendAqiJob
{
    private readonly ILogger<SendAqiJob> _logger;
    private readonly IAqiDataProcessor _aqiDataProcessor;
    
    public SendAqiJob(ILogger<SendAqiJob> logger, IAqiDataProcessor aqiDataProcessor)
    {
        _logger = logger;
        _aqiDataProcessor = aqiDataProcessor;
    }
    
    public async Task Execute(Message message, IJobCancellationToken token)
    {
        _logger.LogInformation($"SendAqiJob started...");

        try
        {
            await _aqiDataProcessor.AqiProcess(message, token.ShutdownToken);
        }
        catch (Exception ex)
        {
            _logger.LogError(ex, "SendAqiJob error: {msg}", ex.Message);
        }

        _logger.LogInformation($"SendAqiJob end...");
    }
}