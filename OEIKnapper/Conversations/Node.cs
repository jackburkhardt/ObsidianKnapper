using Newtonsoft.Json;
using Newtonsoft.Json.Linq;

namespace OEIKnapper;

public abstract class Node
{
    public int NodeID;
    public List<DialogueLink> Links;
    //todo: ExtendedProperties
    public List<ConditionalCall> Conditionals;

    public static Node TryParse(JToken json)
    {
        if (!OEIJsonUtils.ValidateObject(json, "Links"))
        {
            throw new ArgumentException("Unable to parse Node from: " + json.ToString(Formatting.None));
        }

        Node newNode;
        switch ((string)json["$type"])
        {
            case "OEIFormats.FlowCharts.Conversations.TalkNode, OEIFormats":
                newNode = TalkNode.TryParse(json);
                break;
            case "OEIFormats.FlowCharts.Conversations.PlayerResponseNode, OEIFormats":
                newNode = PlayerResponseNode.TryParse(json);
                break;
            case "OEIFormats.FlowCharts.Conversations.ScriptNode, OEIFormats":
                newNode = ScriptNode.TryParse(json);
                break;
            default:
                throw new ArgumentException("Unknown node type: " + json["$type"]);
        }

        newNode.NodeID = json["NodeID"]?.Value<int>() ?? -1;
        newNode.Links = json["Links"].Select(DialogueLink.TryParse).ToList();
        newNode.Conditionals = json["Conditionals"]?.Select(ConditionalCall.TryParse).ToList() ??
                               [];
        
        return newNode;
    }
}