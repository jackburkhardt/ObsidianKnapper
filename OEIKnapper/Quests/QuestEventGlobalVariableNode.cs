﻿using Newtonsoft.Json.Linq;
using OEIKnapper.Conversations;
using ComparisonType = OEIKnapper.Conversations.Conditional.ComparisonType;

namespace OEIKnapper.Quests;

public class QuestEventGlobalVariableNode : Node
{
    public record ConditionalVariable(Guid VariableID, ComparisonType Operator, string VariableValue);

    public ConditionalVariable Conditional;
    public string? ChildFailConditional;
    
    public static QuestEventGlobalVariableNode TryParse(JToken json)
    {
        var condVar = new ConditionalVariable(
            json["Conditional"]["VariableID"].ToObject<Guid>(),
            OEIJsonUtils.ParseEnum(json["Conditional"]["Operator"], ComparisonType.EQUAL),
            json["Conditional"]["VariableValue"]?.Value<string>() ?? ""
        );
        
        return new QuestEventGlobalVariableNode
        {
            Conditional = condVar,
            ChildFailConditional = json["ChildFailConditional"]?.Value<string>() ?? ""
        };
    }
}