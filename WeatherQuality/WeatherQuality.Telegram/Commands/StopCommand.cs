using Botticelli.Framework.Commands;

namespace WeatherQuality.Telegram.Commands;

public class StopCommand : ICommand
{
    public Guid Id { get; }
}