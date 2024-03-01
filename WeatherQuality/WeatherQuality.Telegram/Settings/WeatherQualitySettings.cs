namespace WeatherQuality.Telegram.Settings;

/// <summary>
///     Settings for a messaging sample
/// </summary>
public class WeatherQualitySettings
{
    public string? ChatId { get; set; }
    public string? SecureStorageConnectionString { get; set; }
    public string? DbConnectionString { get; set; }
    public string? BotName { get; set; }

    /// <summary>
    /// Radius for geo caching, km (= 0.62 mile)
    /// </summary>
    public double GeoCachingRadius { get; set; } = 2.0;
    
    /// <summary>
    /// Caching period in hours
    /// </summary>
    public double CachingPeriod { get; set; } = 1.0;

    /// <summary>
    /// Criterias
    /// </summary>
    public List<MetricCriteria> Criteria { get; set; }
}