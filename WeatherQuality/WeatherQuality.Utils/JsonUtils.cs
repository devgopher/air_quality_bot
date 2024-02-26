using System.Text;
using System.Text.Json;
using System.Text.Json.Serialization.Metadata;

namespace WeatherQuality;

public static class JsonUtils
{
    public static string? SerializeToString(this object? src)
        => src != default ? Encoding.UTF8.GetString(JsonSerializer.SerializeToUtf8Bytes(src)) : default;

    public static object? DeserializeFromString(this string? src, Type type) 
        => src == default ? null : JsonSerializer.Deserialize(Encoding.UTF8.GetBytes(src), type);
}