using Microsoft.Extensions.Caching.Memory;
using Microsoft.Extensions.Logging;
using Microsoft.Extensions.Options;
using Nominatim.API.Models;
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

    public async Task<string?> GetCountry(double lat, double lng) => (await InnerGetAddress(lat, lng))?.Address.Country;

    public async Task<string?> GetRegion(double lat, double lng) => (await InnerGetAddress(lat, lng))?.Address.Region;

    public async Task<string?> GetLocality(double lat, double lng) => (await InnerGetAddress(lat, lng))?.Address.County;
    public async Task<string?> GetFullAddress(double lat, double lng) => (await InnerGetAddress(lat, lng))?.DisplayName;
    
    private async Task<GeocodeResponse?> InnerGetAddress(double lat, double lng)
    {
        if (_cache.TryGetValue((lat, lng), out GeocodeResponse? response)) 
            return response;
        response = await _service.InnerGetAddress(lat, lng);
        _cache.Set((lat, lng), response, _settings.Value.Expiration ?? TimeSpan.FromDays(1));

        return response;
    }
}