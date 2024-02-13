using System.Text.Json.Serialization;

namespace WeatherQuality.Domain.Response;

public class Hourly
{
    [JsonPropertyName("time")] public List<string> Time { get; set; }

    [JsonPropertyName("pm10")] public List<double> Pm10 { get; set; }

    [JsonPropertyName("pm2_5")] public List<double> Pm25 { get; set; }

    [JsonPropertyName("carbon_monoxide")] public List<double> CarbonMonoxide { get; set; }

    [JsonPropertyName("nitrogen_dioxide")] public List<double> NitrogenDioxide { get; set; }

    [JsonPropertyName("sulphur_dioxide")] public List<double> SulphurDioxide { get; set; }

    [JsonPropertyName("aerosol_optical_depth")]
    public List<double> AerosolOpticalDepth { get; set; }

    [JsonPropertyName("dust")] public List<double> Dust { get; set; }

    [JsonPropertyName("uv_index")] public List<double> UvIndex { get; set; }

    [JsonPropertyName("uv_index_clear_sky")]
    public List<double> UvIndexClearSky { get; set; }

    [JsonPropertyName("alder_pollen")] public List<double?> AlderPollen { get; set; }

    [JsonPropertyName("birch_pollen")] public List<double?> BirchPollen { get; set; }

    [JsonPropertyName("grass_pollen")] public List<double?> GrassPollen { get; set; }

    [JsonPropertyName("mugwort_pollen")] public List<double?> MugwortPollen { get; set; }

    [JsonPropertyName("olive_pollen")] public List<double?> OlivePollen { get; set; }

    [JsonPropertyName("ragweed_pollen")] public List<double?> RagweedPollen { get; set; }

    [JsonPropertyName("european_aqi")] public List<int> EuropeanAqi { get; set; }
    
    [JsonPropertyName("current")] public Current Current { get; set; }
}