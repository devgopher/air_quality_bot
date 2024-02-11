﻿namespace WeatherQuality.Domain.Request;

/// <summary>
/// Request to OpenMeteo
/// Example : https://air-quality-api.open-meteo.com/v1/air-quality?latitude=55.7422&amp;longitude=37.4023&amp;current=european_aqi&amp;hourly=pm10,pm2_5,carbon_monoxide,nitrogen_dioxide,sulphur_dioxide,
/// ozone,aerosol_optical_depth,dust,uv_index,uv_index_clear_sky,alder_pollen,birch_pollen,grass_pollen,mugwort_pollen,olive_pollen,ragweed_pollen,european_aqi&amp;timezone=Europe%2FMoscow
/// </summary>
public class Request
{
    public decimal Latitude { get; set; }
    public decimal Longitude{ get; set; }
    public string Current { get; set; } = "european_aqi";
    public string Hourly =
        "pm10,pm2_5,carbon_monoxide,nitrogen_dioxide,sulphur_dioxide,ozone,aerosol_optical_depth,dust,uv_index,uv_index_clear_sky,alder_pollen,birch_pollen,grass_pollen,mugwort_pollen,olive_pollen,ragweed_pollen,european_aqi";
    public string Timezone { get; set; } // example: Europe%2FMoscow
}