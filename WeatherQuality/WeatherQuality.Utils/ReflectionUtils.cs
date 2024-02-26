using System.Reflection;
using System.Text.Json.Serialization;

namespace WeatherQuality;

public static class ReflectionUtils
{
    public static Dictionary<string, object> GetJsonProperties(this object src, bool filterDefaultValues = false)
    {
        var dict = new Dictionary<string, object>();
        
        foreach (var prop in src.GetType().GetProperties())
        {
            foreach (var attr in prop.GetCustomAttributes(true))
            {
                if (attr is not JsonPropertyNameAttribute jsonAttr) 
                    continue;
                
                var propName = jsonAttr.Name;
                var propValue = prop.GetValue(src);
                
                if (filterDefaultValues && propValue == default)
                    continue;
                
                dict.Add(propName, propValue);
            }
        }

        return dict;
    }

    public static IEnumerable<string> GetJsonPropertyNames(this Type type)
    {
        foreach (var prop in type.GetProperties())
        {
            foreach (var attr in prop.GetCustomAttributes(true))
            {
                if (attr is not JsonPropertyNameAttribute jsonAttr) 
                    continue;
                
                yield return jsonAttr.Name;
            }
        }
    }
    
    
    public static void SetJsonDeserializedProperty(this object tgt, string jsonName, string value)
    {
        foreach (var prop in tgt.GetType().GetProperties())
        {
            foreach (var attr in prop.GetCustomAttributes(true))
            {
                if (attr is not JsonPropertyNameAttribute jsonAttr || jsonAttr.Name != jsonName) 
                    continue;

                var targetValue = value.DeserializeFromString(prop.PropertyType);
                
                prop.SetValue(tgt, targetValue);
                break;
            }
        }
    }

    public static void MergeByDefaultValues(this object tgt, object src)
    {
        if (tgt.GetType() != src.GetType())
            return;

        var props = tgt.GetType().GetProperties(BindingFlags.Public);

        foreach (var prop in props)
        {
            var srcVal = prop.GetValue(src);
           
            if (srcVal != default)
                continue;
            
            var tgtVal = prop.GetValue(tgt);                
            prop.SetValue(src, tgtVal);
        }
    }
}