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
                                            depthInHours,
                                            token);

        if (cached?.Value is not null) 
            return cached;

        cached = new GeoCacheModel
        {
            Id = Guid.NewGuid(),
            Timestamp = DateTime.UtcNow,
            Latitude = lat,
            Longitude = longitude,
            ElementName = element,
            Value = value
        };

        await _context.AddAsync(cached, token);

        await _context.SaveChangesAsync(token);

        return cached;
    }

    private async Task<GeoCacheModel?> FindInCacheAsync(string element,
                                                        decimal lat,
                                                        decimal longitude,
                                                        decimal radius,
                                                        double depthInHours,
                                                        CancellationToken token) =>
            await _context.GeoCacheModels
                          .FirstOrDefaultAsync(c => DateTime.UtcNow - c.Timestamp < TimeSpan.FromHours(depthInHours) &&
                                                       c.ElementName == element &&
                                                       CalculateDistance(c.Latitude,
                                                                         c.Longitude,
                                                                         lat,
                                                                         longitude) <
                                                       radius,
                                               token);

    private decimal CalculateDistance(decimal srcLat,
                                     decimal srcLong,
                                     decimal tgtLat,
                                     decimal tgtLong)
    {
        var d1 = srcLat * (decimal) (Math.PI / 180.0);
        var num1 = srcLong * (decimal) (Math.PI / 180.0);
        var d2 = tgtLat * (decimal) (Math.PI / 180.0);
        var num2 = tgtLong * (decimal) (Math.PI / 180.0) - num1;
        var d3 = Math.Pow(Math.Sin((double) (d2 - d1) / 2.0), 2.0) +
                 Math.Cos((double) d1) * Math.Cos((double) d2) * Math.Pow(Math.Sin((double) num2 / 2.0), 2.0);

        return (decimal) (6376500.0 * (2.0 * Math.Atan2(Math.Sqrt(d3), Math.Sqrt(1.0 - d3))));
    }
}