using System.ComponentModel.DataAnnotations;

namespace WeatherQuality.Telegram.Database.Models;

public class AirQualityCacheModel
{
    [Key]
    public Guid Id { get; set; }
    public string ChatId { get; set; }
    
    public DateTime Timestamp { get; set; }
    public string SerializedResponse { get; set; }

    /// <summary>
    /// Radius in km
    /// </summary>
    public double Radius { get; set; } = 2.0;
}