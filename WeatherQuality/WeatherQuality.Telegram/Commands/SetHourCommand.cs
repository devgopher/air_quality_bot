using Botticelli.Framework.Commands;

namespace WeatherQuality.Telegram.Commands;

public class SetHourCommand : ICommand
{
    public Guid Id { get; }
}