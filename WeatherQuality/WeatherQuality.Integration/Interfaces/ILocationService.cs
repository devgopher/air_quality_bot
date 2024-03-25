namespace WeatherQuality.Integration.Interfaces
{
    public interface ILocationService
    {
        public Task<string?> GetFullAddress(double lat, double lng);

        public Task<TimeZoneInfo?> GetTimeZone(double lat, double lng);
    }
}