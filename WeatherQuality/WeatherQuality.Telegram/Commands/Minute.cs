using Botticelli.Framework.Commands;

namespace WeatherQuality.Telegram.Commands;

public class Minute : ICommand
{
    public Guid Id { get; }
}