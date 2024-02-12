using Botticelli.Framework.Commands;

namespace WeatherQuality.Telegram.Commands;

public class GetAirQualityCommand : ICommand
{
    public Guid Id { get; }
}