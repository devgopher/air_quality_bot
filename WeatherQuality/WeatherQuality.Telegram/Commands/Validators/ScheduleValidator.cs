using System.Text.RegularExpressions;
using Botticelli.Framework.Commands.Validators;

namespace WeatherQuality.Telegram.Commands.Validators;

public class ScheduleValidator : ICommandValidator<ScheduleCommand>
{
    private readonly Regex _schedRegex = new("^([0-1]?[0-9]|2[0-3]):[0-5][0-9]$");

    public async Task<bool> Validate(List<string> chatIds, string body)
    {
        if (string.IsNullOrWhiteSpace(body))
            return false;
        
        var args = body.Split(' ');

        return args.Length == 2 && _schedRegex.IsMatch(args[1]);
    }

    public string Help() => "Please, use format 'HH24:mm' (for example: '12:15', or '9:07')";
}