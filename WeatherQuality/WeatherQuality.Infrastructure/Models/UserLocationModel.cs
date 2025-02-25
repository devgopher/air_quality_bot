using System.ComponentModel.DataAnnotations;

namespace WeatherQuality.Infrastructure.Models;

public class UserLocationModel
{
    [Key]
    public string ChatId { get; set; }
    public float Latitude { get; set; }
    public float Longitude { get; set; }
    public string? TimeZone { get; set; }
}
