namespace WeatherQuality.Integration
{
    public interface ILocationService
    {
        public Task<string?> GetFullAddress(double lat, double lng);
    }
}