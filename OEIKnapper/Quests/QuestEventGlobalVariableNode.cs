using Newtonsoft.Json.Linq;
using OEIKnapper.Conversations;
using ComparisonType = OEIKnapper.Conversations.Conditional.ComparisonType;

namespace OEIKnapper.Quests;

public class QuestEventGlobalVariableNode : Node
{
    public record ConditionalVariable(Guid VariableID, Conditional.ComparisonType Operation, string VariableValue);

    public ConditionalVariable Conditional;
    public string ChildFailConditionalValue;
    
    public static QuestEventGlobalVariableNode TryParse(JToken json)
    {
        var condVar = new ConditionalVariable(
            json["Conditional"]["VariableID"].ToObject<Guid>(),
            OEIJsonUtils.ParseEnum(json["Conditional"] as JProperty, ComparisonType.EQUAL),
            json["Conditional"]["VariableValue"].Value<string>()
        );
        
        return new QuestEventGlobalVariableNode
        {
            Conditional = condVar,
            ChildFailConditionalValue = json["ChildFailConditionalValue"].Value<string>()
        };
    }
}