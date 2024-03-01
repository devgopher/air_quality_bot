using WeatherQuality.Domain.Request;
using WeatherQuality.Domain.Response;

namespace WeatherQuality.Integration;

public interface IAirQualityIntegration
{
    /// <summary>
    /// Gets air quality results
    /// </summary>
    /// <param name="request"></param>
    /// <returns></returns>
    Task<Response> GetAirQualityAsync(Request request);
}