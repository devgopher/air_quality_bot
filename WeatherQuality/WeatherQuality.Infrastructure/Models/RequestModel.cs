using System.ComponentModel.DataAnnotations;
using System.Text.Json.Serialization;

namespace WeatherQuality.Infrastructure.Models;

public class RequestModel
{
    [Key]
    public Guid Id { get; set; }
    public decimal? Latitude { get; set; }
    public decimal? Longitude { get; set; }
    [JsonIgnore]
    public List<string> Current { get; set; }
    [JsonIgnore]
    public List<string>  Hourly { get; set; }
    public string Timezone { get; set; }
}