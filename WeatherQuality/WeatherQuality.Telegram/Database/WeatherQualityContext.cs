using Microsoft.EntityFrameworkCore;
using WeatherQuality.Telegram.Database.Models;

namespace WeatherQuality.Telegram.Database;

public class WeatherQualityContext : DbContext
{ 
    public WeatherQualityContext(DbContextOptions<WeatherQualityContext> options) : base(options)
    {}

    public DbSet<AirQualityCacheModel> AirQualityCacheModels { get; set; }
    public DbSet<AirQualityCacheDetailsModel> AirQualityCacheDetailsModels { get; set; }
    public DbSet<RequestModel> RequestModels { get; set; }
    public DbSet<UserLocationModel> UserLocationModels { get; set; }
}