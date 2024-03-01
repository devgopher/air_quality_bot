namespace WeatherQuality.Telegram.Settings;

/// <summary>
/// Criteria + text + image
/// </summary>
public class MetricCriteria
{
    public int LowBorder { get; set; }
    public int HiBorder { get; set; }
    public string Text { get; set; }
    
    public List<string> Recommendations { get; set; }
    
    public string ImagePath { get; set; }
    
    public string Color { get; set; } 
}