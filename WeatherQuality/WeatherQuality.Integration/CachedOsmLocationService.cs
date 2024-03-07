using Microsoft.Extensions.Caching.Memory;
using Microsoft.Extensions.Logging;
using Microsoft.Extensions.Options;
using Nominatim.API.Models;
using WeatherQuality.Integration.Interfaces;
using WeatherQuality.Integration.Settings;
using IHttpClientFactory = System.Net.Http.IHttpClientFactory;

namespace WeatherQuality.Integration;

public class CachedOsmLocationService : ILocationService
{
    private readonly OsmLocationService _service;
    private readonly IMemoryCache _cache;
    private readonly IOptionsSnapshot<LocationCacheSettings> _settings;

    public CachedOsmLocationService(ILogger<OsmLocationService> logger, IHttpClientFactory? httpClientFactory, IMemoryCache cache, IOptionsSnapshot<LocationCacheSettings> settings)
    {
        _service = new OsmLocationService(logger, httpClientFactory);
        _cache = cache;
        _settings = settings;
    }

    public async Task<string?> GetFullAddress(double lat, double lng) => (await InnerGetAddress(lat, lng))?.DisplayName;
    public async Task<IEnumerable<string>?> Search(string query, int limitResults)
        => (await InnerSearch(query, limitResults))?.Select(g => g.DisplayName);

    private async Task<IEnumerable<GeocodeResponse>?> InnerSearch(string query, int limitResults = 5)
    {
        if (_cache.TryGetValue(query, out IEnumerable<GeocodeResponse>? response)) 
            return response;
        
        response = (await _service.InnerSearch(query, limitResults))
            .ToArray();

        _cache.Set(query, response, _settings.Value.Expiration ?? TimeSpan.FromDays(1));

        return response;
    }
    
    private async Task<GeocodeResponse?> InnerGetAddress(double lat, double lng)
    {
        if (_cache.TryGetValue((lat, lng), out GeocodeResponse? response)) 
            return response;
        
        response = await _service.InnerGetAddress(lat, lng);
        _cache.Set((lat, lng), response, _settings.Value.Expiration ?? TimeSpan.FromDays(1));

        return response;
    }
}