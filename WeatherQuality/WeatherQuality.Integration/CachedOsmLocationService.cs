using GeoTimeZone;
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
    private static readonly TimeSpan Expiration = TimeSpan.FromDays(1); 

    public CachedOsmLocationService(ILogger<OsmLocationService> logger, IHttpClientFactory? httpClientFactory,
        IMemoryCache cache, IOptionsSnapshot<LocationCacheSettings> settings)
    {
        _service = new OsmLocationService(logger, httpClientFactory);
        _cache = cache;
        _settings = settings;
    }

    public async Task<string?> GetFullAddress(double lat, double lng) => (await InnerGetAddress(lat, lng))?.DisplayName;
    public async Task<TimeZoneInfo?> GetTimeZone(double lat, double lng) => await InnerGetTimeZone(lat, lng);

    private async Task<GeocodeResponse?> InnerGetAddress(double lat, double lng)
    {
        if (_cache.TryGetValue(GetAddrKey(lat, lng), out GeocodeResponse? response))
            return response;

        response = await _service.InnerGetAddress(lat, lng);

        _cache.Set(GetAddrKey(lat, lng), response, _settings.Value.Expiration ?? Expiration);

        return response;
    }

    private static string GetAddrKey(double lat, double lng) => $"addr_{lat}_{lng}";
    private static string GetTzKey(double lat, double lng) => $"tz_{lat}_{lng}";

    private async Task<TimeZoneInfo?> InnerGetTimeZone(double lat, double lng)
    {
        if (_cache.TryGetValue(GetTzKey(lat, lng), out TimeZoneInfo? response))
            return response;

        response = await _service.GetTimeZone(lat, lng);

        _cache.Set(GetTzKey(lat, lng), response, _settings.Value.Expiration ?? Expiration);

        return response;
    }
}