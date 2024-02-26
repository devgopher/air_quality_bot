using Geolocation;
using Microsoft.EntityFrameworkCore;
using WeatherQuality.Infrastructure.Models;

namespace WeatherQuality.Infrastructure;

public class GeoCacheExplorer
{
    private readonly WeatherQualityContext _context;

    public GeoCacheExplorer(WeatherQualityContext context) => _context = context;

    public async Task<GeoCacheModel?> UpsertToCacheAsync(string element,
                                                         decimal lat,
                                                         decimal longitude,
                                                         decimal radius,
                                                         double depthInHours,
                                                         object value,
                                                         CancellationToken token)
    {
        var cached = await FindInCacheAsync(element,
                                            lat,
                                            longitude,
                                            radius,
                                            depthInHours);

        if (cached != default)
            return cached;
        
        if (value is null)
            return default;
        
        cached = new GeoCacheModel
        {
            Id = Guid.NewGuid(),
            Timestamp = DateTime.UtcNow,
            Latitude = lat,
            Longitude = longitude,
            ElementName = element,
            SerializedValue = value.SerializeToString()!
        };

        await _context.AddAsync(cached, token);

        await _context.SaveChangesAsync(token);

        return cached;
    }

    private async Task<GeoCacheModel?> FindInCacheAsync(string element,
        decimal lat,
        decimal longitude,
        decimal radius,
        double depthInHours)
    {
        var models = _context.GeoCacheModels
            .Where(c => DateTime.UtcNow - c.Timestamp < TimeSpan.FromHours(depthInHours) &&
                        c.ElementName == element)
            .ToArray();

        var nearest= models.FirstOrDefault(c => CalculateDistance(c.Latitude,
            c.Longitude,
            lat,
            longitude) < radius);

        return nearest;
    }

    private decimal CalculateDistance(decimal srcLat,
                                     decimal srcLong,
                                     decimal tgtLat,
                                     decimal tgtLong)
    {
       return (decimal)GeoCalculator.GetDistance((double)srcLat, (double)srcLong, (double)tgtLat, (double)tgtLong, 1, DistanceUnit.Kilometers);
    }
}