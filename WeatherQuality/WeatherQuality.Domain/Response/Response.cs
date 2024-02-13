using System.Text.Json.Serialization;

namespace WeatherQuality.Domain.Response;

/// <summary>
/// An OpenMeteo response
/// </summary>
public class Response
{
    public bool IsSuccess { get; set; }

    public string Error { get; set; }

    [JsonPropertyName("latitude")] public double Latitude { get; set; }

    [JsonPropertyName("longitude")] public double Longitude { get; set; }

    [JsonPropertyName("generationtime_ms")]
    public double GenerationtimeMs { get; set; }

    [JsonPropertyName("utc_offset_seconds")]
    public int UtcOffsetSeconds { get; set; }

    [JsonPropertyName("timezone")] public string Timezone { get; set; }

    [JsonPropertyName("timezone_abbreviation")]
    public string TimezoneAbbreviation { get; set; }

    [JsonPropertyName("elevation")] public double Elevation { get; set; }

    [JsonPropertyName("hourly_units")] public HourlyUnits HourlyUnits { get; set; }

    [JsonPropertyName("hourly")] public Hourly Hourly { get; set; }
}