using Newtonsoft.Json.Linq;

namespace OEIKnapper;

public static class JsonFieldValidate
{
    public static bool ValidateObject(JToken json, params string[] fields)
    {
        return fields.All(field => json[field] != null);
    }
}