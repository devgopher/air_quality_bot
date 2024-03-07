namespace WeatherQuality.Integration.Interfaces
{
    public interface ILocationService
    {
        public Task<string?> GetFullAddress(double lat, double lng);
        public Task<IEnumerable<string>?> Search(string query, int limitResults = 5);
    }
}