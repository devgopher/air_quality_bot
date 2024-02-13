using System.ComponentModel.DataAnnotations;

namespace WeatherQuality.Telegram.Database.Models;

public class UserLocationModel
{
    [Key]
    public Guid ChatId { get; set; }
    public double Latitude { get; set; }
    public double Longitude { get; set; }
}