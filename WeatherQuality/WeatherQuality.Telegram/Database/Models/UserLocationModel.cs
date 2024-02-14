using System.ComponentModel.DataAnnotations;

namespace WeatherQuality.Telegram.Database.Models;

public class UserLocationModel
{
    [Key]
    public string ChatId { get; set; }
    public decimal? Latitude { get; set; }
    public decimal? Longitude { get; set; }
}