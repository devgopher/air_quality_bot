using System.Text.Json.Serialization;
using Newtonsoft.Json;

namespace WeatherQuality.Domain.Response;

public class HourlyUnits
{
    [JsonPropertyName("time")]
    [JsonProperty("time")]
    public string Time { get; set; }

    [JsonPropertyName("pm10")]
    [JsonProperty("pm10")]
    public string Pm10 { get; set; }

    [JsonPropertyName("pm2_5")]
    [JsonProperty("pm2_5")]
    public string Pm25 { get; set; }

    [JsonPropertyName("carbon_monoxide")]
    [JsonProperty("carbon_monoxide")]
    public string CarbonMonoxide { get; set; }

    [JsonPropertyName("nitrogen_dioxide")]
    [JsonProperty("nitrogen_dioxide")]
    public string NitrogenDioxide { get; set; }

    [JsonPropertyName("sulphur_dioxide")]
    [JsonProperty("sulphur_dioxide")]
    public string SulphurDioxide { get; set; }

    [JsonPropertyName("aerosol_optical_depth")]
    [JsonProperty("aerosol_optical_depth")]
    public string AerosolOpticalDepth { get; set; }

    [JsonPropertyName("dust")]
    [JsonProperty("dust")]
    public string Dust { get; set; }

    [JsonPropertyName("uv_index")]
    [JsonProperty("uv_index")]
    public string UvIndex { get; set; }

    [JsonPropertyName("uv_index_clear_sky")]
    [JsonProperty("uv_index_clear_sky")]
    public string UvIndexClearSky { get; set; }

    [JsonPropertyName("alder_pollen")]
    [JsonProperty("alder_pollen")]
    public string AlderPollen { get; set; }

    [JsonPropertyName("birch_pollen")]
    [JsonProperty("birch_pollen")]
    public string BirchPollen { get; set; }

    [JsonPropertyName("grass_pollen")]
    [JsonProperty("grass_pollen")]
    public string GrassPollen { get; set; }

    [JsonPropertyName("mugwort_pollen")]
    [JsonProperty("mugwort_pollen")]
    public string MugwortPollen { get; set; }

    [JsonPropertyName("olive_pollen")]
    [JsonProperty("olive_pollen")]
    public string OlivePollen { get; set; }

    [JsonPropertyName("ragweed_pollen")]
    [JsonProperty("ragweed_pollen")]
    public string RagweedPollen { get; set; }

    [JsonPropertyName("european_aqi")]
    [JsonProperty("european_aqi")]
    public string EuropeanAqi { get; set; }
}