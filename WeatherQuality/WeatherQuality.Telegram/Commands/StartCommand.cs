using Botticelli.Framework.Commands;

namespace WeatherQuality.Telegram.Commands;

public class StartCommand : ICommand
{
    public Guid Id { get; }
}