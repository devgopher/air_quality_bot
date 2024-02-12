using Botticelli.Framework.Commands;

namespace WeatherQuality.Telegram.Commands;

public class GetCurrentLocationCommand : ICommand
{
    public Guid Id { get; }
}