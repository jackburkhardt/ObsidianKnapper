using Newtonsoft.Json.Linq;

namespace OEIKnapper;

public static class OEIJsonUtils
{
    /// <summary>
    /// Checks if all fields are present in the JSON object.
    /// </summary>
    public static bool ValidateObject(JToken json, params string[] fields)
    {
        return fields.All(field => json[field] != null);
    }
}