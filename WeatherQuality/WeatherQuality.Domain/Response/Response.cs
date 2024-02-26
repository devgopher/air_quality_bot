using System.Text.Json.Serialization;
using Newtonsoft.Json;

namespace WeatherQuality.Domain.Response;

/// <summary>
/// An OpenMeteo response
/// </summary>
public class Response
{
    public bool IsSuccess { get; set; }

    public string Error { get; set; }

    [JsonPropertyName("latitude")]
    [JsonProperty("latitude")]
    public double Latitude { get; set; }

    [JsonPropertyName("longitude")]
    [JsonProperty("longitude")]
    public double Longitude { get; set; }

    [JsonPropertyName("generationtime_ms")]
    [JsonProperty("generationtime_ms")]
    public double GenerationtimeMs { get; set; }

    [JsonPropertyName("utc_offset_seconds")]
    [JsonProperty("utc_offset_seconds")]
    public int UtcOffsetSeconds { get; set; }

    [JsonPropertyName("timezone")]
    [JsonProperty("timezone")]
    public string Timezone { get; set; }

    [JsonPropertyName("timezone_abbreviation")]
    [JsonProperty("timezone_abbreviation")]
    public string TimezoneAbbreviation { get; set; }

    [JsonPropertyName("elevation")]
    [JsonProperty("elevation")]
    public double Elevation { get; set; }

    [JsonPropertyName("hourly_units")]
    [JsonProperty("hourly_units")]
    public HourlyUnits HourlyUnits { get; set; }

    [JsonPropertyName("hourly")]
    [JsonProperty("hourly")]
    public Hourly Hourly { get; set; }

    [JsonPropertyName("current")]
    [JsonProperty("current")]
    public Current Current { get; set; }
}