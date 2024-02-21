using Botticelli.Framework.Commands;

namespace WeatherQuality.Telegram.Commands;

public class DetailsCommand : ICommand
{
    public Guid Id { get; }
}