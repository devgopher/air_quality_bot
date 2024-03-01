using System.Text.Json;
using Flurl;
using Flurl.Http;
using Microsoft.Extensions.Logging;
using WeatherQuality.Domain.Request;
using WeatherQuality.Domain.Response;
using WeatherQuality.Integration.Interfaces;

namespace WeatherQuality.Integration;

public class OpenMeteoAirQualityIntegration : IAirQualityIntegration
{
    private ILogger<OpenMeteoAirQualityIntegration> _logger;
    private string _baseUrl = "https://air-quality-api.open-meteo.com/v1/air-quality";

    public OpenMeteoAirQualityIntegration(ILogger<OpenMeteoAirQualityIntegration> logger) => _logger = logger;

    /// <summary>
    /// Gets air quality results
    /// </summary>
    /// <param name="request"></param>
    /// <returns></returns>
    public async Task<Response> GetAirQualityAsync(Request request)
    {
        try
        {
            var result = await _baseUrl
                .SetQueryParam("latitude", request.Latitude)
                .SetQueryParam("longitude", request.Longitude)
                .SetQueryParam("current", string.Join(",", request.Current))
                .GetJsonAsync<Response>();
            result.IsSuccess = true;

            return result;
        }
        catch (Exception ex)
        {
            _logger.LogError($"OpenMeteo integration error: {ex.Message}", ex);

            return new Response
            {
                IsSuccess = false,
                Error = ex.Message
            };
        }
    }
}