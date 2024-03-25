using Botticelli.Shared.ValueObjects;
using WeatherQuality.Infrastructure;
using WeatherQuality.Infrastructure.Models;
using WeatherQuality.Integration.Interfaces;

namespace WeatherQuality.Telegram.Services;

public class GetLocationService : IGetLocationService
{
    private readonly ILocationService _locationService;
    private readonly IServiceProvider _sp;

    public GetLocationService(IServiceProvider sp, ILocationService locationService)
    {
        _sp = sp;
        _locationService = locationService;
    }

    public UserLocationModel? GetLocation(Message message)
    {
        var context = _sp.GetService<WeatherQualityContext>();

        return context.UserLocationModels.FirstOrDefault(um => message.ChatIds.Contains(um.ChatId));
    }

    public async Task<string?> GetAddress(UserLocationModel location) =>
        await _locationService.GetFullAddress(location.Latitude, location.Longitude);

    public async Task<TimeZoneInfo?> GetTimeZone(UserLocationModel location) 
        => await _locationService.GetTimeZone(location.Latitude, location.Longitude);
}