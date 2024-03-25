using Botticelli.Shared.ValueObjects;

namespace WeatherQuality.Telegram.Services;

public interface IAqiDataProcessor 
{
    public Task AqiProcess(Message message, CancellationToken token);
}