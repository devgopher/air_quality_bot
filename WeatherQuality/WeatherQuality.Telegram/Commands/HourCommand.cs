using Botticelli.Framework.Commands;

namespace WeatherQuality.Telegram.Commands;

public class HourCommand : ICommand
{
    public Guid Id { get; }
}