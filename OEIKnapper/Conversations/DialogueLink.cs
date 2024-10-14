using Newtonsoft.Json;
using Newtonsoft.Json.Linq;

namespace OEIKnapper;

public class DialogueLink
{
    public int FromNodeID;
    public int ToNodeID;
    public List<ConditionalCall> Conditionals;
    //todo: Extension properties

    public static DialogueLink TryParse(JToken json)
    {
        if (!OEIJsonUtils.ValidateObject(json, "FromNodeID", "ToNodeID", "Conditionals"))
        {
            throw new ArgumentException("Unable to parse DialogueLink from: " + json.ToString(Formatting.None));
        }
        
        return new DialogueLink
        {
            FromNodeID = json["FromNodeID"].Value<int>(),
            ToNodeID = json["ToNodeID"].Value<int>(),
           // Conditionals = json["Conditionals"].Select(ConditionalCall.TryParse).ToList()
        };
    }
}