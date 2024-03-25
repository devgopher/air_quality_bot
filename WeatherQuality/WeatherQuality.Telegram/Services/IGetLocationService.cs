using Botticelli.Shared.ValueObjects;
using WeatherQuality.Infrastructure.Models;

namespace WeatherQuality.Telegram.Services;

public interface IGetLocationService
{
    public UserLocationModel? GetLocation(Message message);
    public Task<string?> GetAddress(UserLocationModel location);
    
    public Task<TimeZoneInfo?> GetTimeZone(UserLocationModel location);
}