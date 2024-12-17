using Newtonsoft.Json;
using Newtonsoft.Json.Linq;
using OEIKnapper.Conversations;
using OEIKnapper.Quests;

namespace OEIKnapper;

public abstract class Node
{
    public int NodeID = -1;
    public List<NodeLink> Links = [];
    public List<string> ExtendedProperties = [];
    [JsonProperty(DefaultValueHandling = DefaultValueHandling.Ignore)]
    public ConditionalExpression Conditionals = new ();
    public List<NodeScriptItem> OnEnterScripts = [];
    public List<NodeScriptItem> OnExitScripts = [];
    public List<NodeScriptItem> OnUpdateScripts = [];

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
            case "OEIFormats.FlowCharts.BankNode, OEIFormats":
                newNode = BankNode.TryParse(json);
                break;
            case "OEIFormats.FlowCharts.Quests.GlobalQuestEventGlobalVariableNode, OEIFormats":
                newNode = QuestEventGlobalVariableNode.TryParse(json);
                break;
            case "OEIFormats.FlowCharts.Quests.GlobalVariableObjectiveNode, OEIFormats":
                newNode = GlobalVariableObjectiveNode.TryParse(json);
                break;
            case "OEIFormats.FlowCharts.Quests.QuestNode, OEIFormats":
                newNode = QuestNode.TryParse(json);
                break;
            case "OEIFormats.FlowCharts.Quests.GlobalQuestNode, OEIFormats":
                newNode = GlobalQuestNode.TryParse(json);
                break;
            default:
                throw new ArgumentException("Unknown node type: " + json["$type"]);
        }

        newNode.NodeID = json["NodeID"]?.Value<int>() ?? -1;
        newNode.Links = json["Links"].Select(NodeLink.TryParse).ToList();
        newNode.Conditionals = Conditional.TryParse(json["Conditionals"]);
        newNode.ExtendedProperties = json["ClassExtender"]["ExtendedProperties"].ToObject<List<string>>();
        newNode.OnEnterScripts = json["OnEnterScripts"].Select(NodeScriptItem.TryParse).ToList();
        newNode.OnExitScripts = json["OnExitScripts"].Select(NodeScriptItem.TryParse).ToList();
        newNode.OnUpdateScripts = json["OnUpdateScripts"].Select(NodeScriptItem.TryParse).ToList();
        
        return newNode;
    }
}