using System.ComponentModel.DataAnnotations;

namespace WeatherQuality.Infrastructure.Models;

public class RequestModel
{
    [Key]
    public Guid Id { get; set; }
    public decimal? Latitude { get; set; }
    public decimal? Longitude { get; set; }
    public List<string> Current { get; set; }
    public List<string>  Hourly { get; set; }
    public string Timezone { get; set; }
}