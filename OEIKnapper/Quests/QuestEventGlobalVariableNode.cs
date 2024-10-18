using Newtonsoft.Json.Linq;
using OEIKnapper.Conversations;

namespace OEIKnapper.Quests;

public class QuestEventGlobalVariableNode : Node
{
    public record ConditionalVariable(Guid VariableID, Conditional.ComparisonType Operation, string VariableValue);

    public ConditionalVariable Conditional;
    public string ChildFailConditionalValue;
    
    public static QuestEventGlobalVariableNode TryParse(JToken json)
    {
        return new QuestEventGlobalVariableNode
        {
            Conditional = new ConditionalVariable(
                json["Conditional"]["VariableID"].ToObject<Guid>(),
                (Conditional.ComparisonType)json["Conditional"]["Operation"]?.Value<int>(),
                json["Conditional"]["VariableValue"].Value<string>()
            ),
            ChildFailConditionalValue = json["ChildFailConditionalValue"].Value<string>()
        };
    }
}