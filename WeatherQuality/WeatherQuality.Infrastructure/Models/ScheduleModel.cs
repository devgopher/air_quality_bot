using System.ComponentModel.DataAnnotations;

namespace WeatherQuality.Infrastructure.Models;

/// <summary>
/// Sets scheduling for sending messages for current location
/// </summary>
public class ScheduleModel
{
    public string? ChatId { get; set; }
    public string? Cron { get; set; }
}