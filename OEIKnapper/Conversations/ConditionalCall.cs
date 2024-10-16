using Newtonsoft.Json.Linq;

namespace OEIKnapper.Conversations;

public class ConditionalCall
{
    public string Function;
    public List<string> Parameters;
    public bool Not;
    
    public static ConditionalCall TryParse(JToken json)
    {
        
        return new ConditionalCall
        {
            Function = json["Data"]["FullName"].Value<string>(),
            Parameters = json["Data"]["Parameters"].Value<List<string>>(),
            Not = json["Not"]?.Value<bool>() ?? false
        };
    }
}