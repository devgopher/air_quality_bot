using System.ComponentModel.DataAnnotations;

namespace WeatherQuality.Infrastructure.Models;

public class UserLocationModel
{
    [Key]
    public string ChatId { get; set; }
    public decimal? Latitude { get; set; }
    public decimal? Longitude { get; set; }
}