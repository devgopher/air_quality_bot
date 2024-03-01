using Microsoft.Extensions.Logging;
using Nominatim.API.Geocoders;
using Nominatim.API.Models;
using Nominatim.API.Web;

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

    public async Task<string?> GetCountry(double lat, double lng)
        => (await InnerGetAddress(lat, lng))?.Address?.Country;

    public async Task<string?> GetRegion(double lat, double lng)
    {
        var address = (await InnerGetAddress(lat, lng))?.Address;
        return $"{address?.Region}, {address?.State}";
    }

    public async Task<string?> GetLocality(double lat, double lng)
    {
        var address = (await InnerGetAddress(lat, lng))?.Address;
        return address?.City ?? address?.Town ?? address?.Village ?? address?.Hamlet;
    }

    public async Task<string?> GetFullAddress(double lat, double lng)
        => (await InnerGetAddress(lat, lng))?.DisplayName;

    public async Task<string?> GetInternationalAddress(double lat, double lng)
    {
        var fullAddress = await InnerGetAddress(lat, lng);

        return fullAddress?.DisplayName;
    }

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