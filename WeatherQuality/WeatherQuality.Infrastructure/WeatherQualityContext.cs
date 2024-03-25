using Microsoft.EntityFrameworkCore;
using WeatherQuality.Infrastructure.Models;

namespace WeatherQuality.Infrastructure;

public class WeatherQualityContext : DbContext
{
    public WeatherQualityContext(DbContextOptions<WeatherQualityContext> options) : base(options)
    {
    }

    protected override void OnModelCreating(ModelBuilder modelBuilder)
    {
        base.OnModelCreating(modelBuilder);

        modelBuilder.Entity<ScheduleModel>().HasKey(sm => new { sm.ChatId, sm.Cron });
    }

    public DbSet<GeoCacheModel> GeoCacheModels { get; set; }
    public DbSet<RequestModel> RequestModels { get; set; }
    public DbSet<UserLocationModel> UserLocationModels { get; set; }
    public DbSet<ScheduleModel> ScheduleModels { get; set; }
}