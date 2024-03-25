using GeoTimeZone;
using Microsoft.Extensions.Logging;
using Nominatim.API.Geocoders;
using Nominatim.API.Models;
using Nominatim.API.Web;
using WeatherQuality.Integration.Interfaces;

namespace WeatherQuality.Integration;

public class OsmLocationService : ILocationService
{
    private readonly ILogger _logger;
    private readonly IHttpClientFactory _httpClientFactory;

    public OsmLocationService(ILogger logger, IHttpClientFactory? httpClientFactory)
    {
        _logger = logger;
        _httpClientFactory = httpClientFactory;
    }

    public OsmLocationService(ILogger<OsmLocationService> logger, IHttpClientFactory? httpClientFactory)
        : this((ILogger)logger, httpClientFactory)
    {
    }

    public async Task<string?> GetFullAddress(double lat, double lng)
        => (await InnerGetAddress(lat, lng))?.DisplayName;

    public Task<TimeZoneInfo?> GetTimeZone(double lat, double lng) 
        => Task.FromResult(TimeZoneInfo.FromSerializedString(TimeZoneLookup.GetTimeZone(lat, lng).Result))!;

    public async Task<GeocodeResponse?> InnerGetAddress(double lat, double lng)
    {
        var nominatimWebInterface = new NominatimWebInterface(_httpClientFactory);

        var geoCoder = new ReverseGeocoder(nominatimWebInterface);

        var response = await geoCoder.ReverseGeocode(new ReverseGeocodeRequest()
        {
            Latitude = lat,
            Longitude = lng
        });

        return response;
    }
}