using Newtonsoft.Json.Linq;

namespace OEIKnapper;

public static class OEIJsonUtils
{
    /// <summary>
    /// Checks if all fields are present in the JSON object.
    /// </summary>
    public static bool ValidateObject(JToken? json, params string[] fields)
    {
        return fields.All(field => json?[field] != null);
    }

    public static T ParseEnum<T>(JToken? json, T defaultValue) where T : struct
    {
        if (json == null || json.Type != JTokenType.Integer)
        {
            return defaultValue;
        }

        return (T) Enum.ToObject(typeof(T), json.Value<int>());
    }
}