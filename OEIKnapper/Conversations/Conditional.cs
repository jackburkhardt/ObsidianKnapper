using Newtonsoft.Json;
using Newtonsoft.Json.Linq;

namespace OEIKnapper.Conversations;

/// <summary>
/// Defines conditions for proceeding to the attached <see cref="Node"/>, <see cref="NodeLink"/>, or <see cref="Quests.QuestNode"/>.
/// This is a parent class for parsing <see cref="ConditionalExpression"/> and <see cref="ConditionalCall"/>.
/// </summary>
public abstract class Conditional
{
    public enum ComparisonType
    {
        AND,
        OR,
        EQUAL,
        NOT_EQUAL,
        GREATER_THAN,
        LESS_THAN,
        GREATER_THAN_OR_EQUAL,
        LESS_THAN_OR_EQUAL
    }
    
    public static ConditionalExpression TryParse(JToken json)
    {
        var cond = new ConditionalExpression
        {
            Conditions = [],
            Operator = OEIJsonUtils.ParseEnum(json["Operator"], ComparisonType.AND)
        };

        if (json["Components"] is JArray)
        {
            foreach (var item in json["Components"])
            {
                var type = item["$type"]?.Value<string>();
                if (type == "OEIFormats.FlowCharts.ConditionalCall, OEIFormats")
                {
                    cond.Conditions.Add(ConditionalCall.TryParse(item));
                }
                else if (type == "OEIFormats.FlowCharts.ConditionalExpression, OEIFormats")
                {
                    cond.Conditions.Add(ConditionalExpression.TryParse(item));
                }
                // switch (item["$type"]?.Value<string>())
                // {
                //     case "OEIFormats.FlowCharts.ConditionalCall, OEIFormats":
                //         cond.Conditions.Add(ConditionalCall.TryParse(item));
                //         break;
                //     case "OEIFormats.FlowCharts.ConditionalExpression, OEIFormats":
                //         cond.Conditions.Add(ConditionalExpression.TryParse(item));
                //         break;
                //     default:
                //         cond.Conditions.Add(ConditionalCall.TryParse(item));
                //         break;
                // }
            }
        }
        
        return cond;
    }   
}