using System.Text.Json.Serialization;
using Newtonsoft.Json;

namespace WeatherQuality.Domain.Response;

public class Current
{
    [JsonProperty("time")]
    public string Time { get; set; }

    [JsonProperty("interval")]
    public int Interval { get; set; }

    [JsonProperty("european_aqi")]
    public int EuropeanAqi { get; set; }

    [JsonProperty("pm10")]
    public double Pm10 { get; set; }

    [JsonProperty("pm2_5")]
    public double Pm25 { get; set; }

    [JsonProperty("carbon_monoxide")]
    public double CarbonMonoxide { get; set; }

    [JsonProperty("nitrogen_dioxide")]
    public double NitrogenDioxide { get; set; }

    [JsonProperty("sulphur_dioxide")]
    public double SulphurDioxide { get; set; }

    [JsonProperty("ozone")]
    public double Ozone { get; set; }

    [JsonProperty("aerosol_optical_depth")]
    public double AerosolOpticalDepth { get; set; }

    [JsonProperty("dust")]
    public double Dust { get; set; }

    [JsonProperty("uv_index")]
    public double UvIndex { get; set; }

    [JsonProperty("uv_index_clear_sky")]
    public double UvIndexClearSky { get; set; }

    [JsonProperty("ammonia")]
    public double Ammonia { get; set; }

    [JsonProperty("alder_pollen")]
    public double AlderPollen { get; set; }

    [JsonProperty("birch_pollen")]
    public double BirchPollen { get; set; }

    [JsonProperty("grass_pollen")]
    public double GrassPollen { get; set; }

    [JsonProperty("mugwort_pollen")]
    public double MugwortPollen { get; set; }

    [JsonProperty("olive_pollen")]
    public double OlivePollen { get; set; }

    [JsonProperty("ragweed_pollen")]
    public double RagweedPollen { get; set; }
}