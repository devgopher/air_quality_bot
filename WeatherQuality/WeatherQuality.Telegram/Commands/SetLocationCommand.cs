using Botticelli.Framework.Commands;

namespace WeatherQuality.Telegram.Commands;

public class SetLocationCommand : ICommand
{
    public Guid Id { get; }
}