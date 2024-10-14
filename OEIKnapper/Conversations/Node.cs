using Newtonsoft.Json;
using Newtonsoft.Json.Linq;

namespace OEIKnapper;

public class Node
{
    public int NodeID;
    public List<DialogueLink> Links;
    //todo: ExtendedProperties
    public List<ConditionalCall> Conditionals;

    public static Node TryParse(JToken json)
    {
        if (!JsonFieldValidate.ValidateObject(json, "NodeID", "Links"))
        {
            throw new ArgumentException("Unable to parse Node from: " + json.ToString(Formatting.None));
        }

        Node newNode = new Node
        {
            NodeID = json["NodeID"].Value<int>(),
            Links = json["Links"].Select(DialogueLink.TryParse).ToList(),
            Conditionals = json["Conditionals"]?.Select(ConditionalCall.TryParse).ToList() ?? new List<ConditionalCall>()
        };


        switch ((string)json["$type"])
        {
            case "OEIFormats.FlowCharts.Conversations.TalkNode, OEIFormats":
                newNode = TalkNode.TryParse(newNode);
                break;
            case "OEIFormats.FlowCharts.Conversations.PlayerResponseNode, OEIFormats":
                newNode = PlayerResponseNode.TryParse(newNode);
                break;
            case "OEIFormats.FlowCharts.Conversations.ScriptNode, OEIFormats":
                newNode = ScriptNode.TryParse(newNode);
                break;
            default:
                throw new ArgumentException("Unknown node type: " + json["$type"]);
        }
        

        return newNode;
        
    }
}