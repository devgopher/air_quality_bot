using System.ComponentModel.DataAnnotations;
using WeatherQuality.Domain.Response;

namespace WeatherQuality.Telegram.Database.Models;

public class RequestModel
{
    [Key]
    public Guid Id { get; set; }
    public decimal? Latitude { get; set; }
    public decimal? Longitude { get; set; }
    public string Current { get; set; }
    public string Hourly { get; set; }
    public string Timezone { get; set; }
}