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

    public async Task<IEnumerable<string>?> Search(string query, int limitResults = 5)
        => (await InnerSearch(query, limitResults))?
            .Select(g => g.DisplayName)
            .ToList();

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
    
    public async Task<IEnumerable<GeocodeResponse>> InnerSearch(string query, int limitResults)
    {
        var nominatimWebInterface = new NominatimWebInterface(_httpClientFactory);
       
        var geoCoder = new ForwardGeocoder(nominatimWebInterface);

        var response = await geoCoder.Geocode(new ForwardGeocodeRequest
        {
            queryString = query,
            LimitResults = limitResults,
        });
        
        return response;
    }
}