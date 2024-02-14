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

    [JsonProperty("latitude")] public double Latitude { get; set; }

    [JsonProperty("longitude")] public double Longitude { get; set; }

    [JsonProperty("generationtime_ms")]
    public double GenerationtimeMs { get; set; }

    [JsonProperty("utc_offset_seconds")]
    public int UtcOffsetSeconds { get; set; }

    [JsonProperty("timezone")] public string Timezone { get; set; }

    [JsonProperty("timezone_abbreviation")]
    public string TimezoneAbbreviation { get; set; }

    [JsonProperty("elevation")] public double Elevation { get; set; }

    [JsonProperty("hourly_units")] public HourlyUnits HourlyUnits { get; set; }

    [JsonProperty("hourly")] public Hourly Hourly { get; set; }
    
    [JsonProperty("current")] public Current Current { get; set; }
}