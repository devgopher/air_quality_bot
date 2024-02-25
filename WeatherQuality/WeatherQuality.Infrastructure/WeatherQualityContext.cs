using Microsoft.EntityFrameworkCore;
using WeatherQuality.Infrastructure.Models;

namespace WeatherQuality.Infrastructure;

public class WeatherQualityContext : DbContext
{ 
    public WeatherQualityContext(DbContextOptions<WeatherQualityContext> options) : base(options)
    {}

    public DbSet<GeoCacheModel> GeoCacheModels { get; set; }
    public DbSet<RequestModel> RequestModels { get; set; }
    public DbSet<UserLocationModel> UserLocationModels { get; set; }
}