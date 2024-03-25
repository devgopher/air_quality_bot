using Botticelli.Framework.Commands;

namespace WeatherQuality.Telegram.Commands;

public class ScheduleCommand : ICommand
{
    public Guid Id { get; }
}

public class CleanScheduleCommand : ICommand
{
    public Guid Id { get; }
}