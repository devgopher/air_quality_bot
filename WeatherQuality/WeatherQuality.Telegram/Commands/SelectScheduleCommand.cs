using Botticelli.Framework.Commands;

namespace WeatherQuality.Telegram.Commands;

public class SelectScheduleCommand : ICommand
{
    public Guid Id { get; }
}