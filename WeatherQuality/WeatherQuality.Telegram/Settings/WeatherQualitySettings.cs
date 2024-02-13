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
}