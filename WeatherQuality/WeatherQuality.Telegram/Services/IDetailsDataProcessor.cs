using Botticelli.Shared.ValueObjects;

namespace WeatherQuality.Telegram.Services;

public interface IDetailsDataProcessor
{
    public Task DetailsProcess(Message message, CancellationToken token);
}