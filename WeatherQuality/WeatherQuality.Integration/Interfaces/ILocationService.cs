namespace WeatherQuality.Integration.Interfaces
{
    public interface ILocationService
    {
        public Task<string?> GetFullAddress(double lat, double lng);
    }
}