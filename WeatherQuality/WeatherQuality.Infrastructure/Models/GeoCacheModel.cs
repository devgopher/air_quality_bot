using System.ComponentModel.DataAnnotations;

namespace WeatherQuality.Infrastructure.Models;

public class GeoCacheModel
{
    [Key]
    public Guid Id { get; set; }
    public DateTime Timestamp { get; set; }
    public float Latitude { get; set; }
    public float Longitude { get; set; }
    public string ElementName { get; set; }
    public string SerializedValue { get; set; }
    public byte[]? BinaryData { get; set; }
}