using Newtonsoft.Json;
using Newtonsoft.Json.Linq;

namespace OEIKnapper.Conversations;

public class ConditionalCall : Conditional
{
    public string Function;
    public List<string> Parameters = [];
    public bool Not;
    
    public static ConditionalCall TryParse(JToken json)
    {
        if (!OEIJsonUtils.ValidateObject(json, "Data"))
        {
            throw new ArgumentException("Unable to parse ConditionalCall from: " + json.ToString(Formatting.None));
        }
        var funcName = json["Data"]["FullName"].Value<string>();
        var parameters = json["Data"]["Parameters"].ToObject<List<string>>();
        
        return new ConditionalCall
        {
            Function = funcName,
            Parameters = parameters,
            Not = json["Not"]?.Value<bool>() ?? false
        };
    }
    
    public override string ToString()
    {
        return $"{(Not ? "!" : "")}{Function}({string.Join(", ", Parameters)})";
    }
}