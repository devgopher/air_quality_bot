namespace WeatherQuality.Integration
{
    public interface ILocationService
    {
        public Task<string?> GetCountry(double lat, double lng);
        public Task<string?> GetRegion(double lat, double lng);
        public Task<string?> GetLocality(double lat, double lng);
        public Task<string?> GetFullAddress(double lat, double lng);
    }
}