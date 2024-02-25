using System.ComponentModel.DataAnnotations;

namespace WeatherQuality.Infrastructure.Models;

public class GeoCacheModel
{
    [Key]
    public Guid Id { get; set; }
    public DateTime Timestamp { get; set; }
    public decimal Latitude { get; set; }
    public decimal Longitude { get; set; }
    public string ElementName { get; set; }
    public object Value { get; set; }
    public byte[] BinaryData { get; set; }
}