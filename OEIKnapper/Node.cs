using Newtonsoft.Json;
using Newtonsoft.Json.Linq;

namespace OEIKnapper.Conversations;

public abstract class Node
{
    public int NodeID;
    public List<NodeLink> Links;
    public List<string> ExtendedProperties;
    public ConditionalExpression Conditionals;

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
        newNode.Links = json["Links"].Select(NodeLink.TryParse).ToList();
        newNode.Conditionals = Conditional.TryParse(json["Conditionals"]);
        newNode.ExtendedProperties = json["ClassExtender"]["ExtendedProperties"].ToObject<List<string>>();
        
        return newNode;
    }
}