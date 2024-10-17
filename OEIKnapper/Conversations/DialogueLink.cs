using Newtonsoft.Json;
using Newtonsoft.Json.Linq;

namespace OEIKnapper.Conversations;

public class DialogueLink
{
    public int FromNodeID;
    public int ToNodeID;
    public ConditionalExpression Conditionals;
    //todo: Extension properties

    public static DialogueLink TryParse(JToken json)
    {
        if (!OEIJsonUtils.ValidateObject(json, "ToNodeID", "Conditionals"))
        {
            throw new ArgumentException("Unable to parse DialogueLink from: " + json.ToString(Formatting.None));
        }
        
        return new DialogueLink
        {
            FromNodeID = json["FromNodeID"]?.Value<int>() ?? -1,
            ToNodeID = json["ToNodeID"].Value<int>(),
            Conditionals = Conditional.TryParse(json["Conditionals"])
        };
    }
}