using Newtonsoft.Json.Linq;

namespace OEIKnapper.Conversations;

public class ConditionalExpression
{
    public ComparisonType Operator;
    public List<ConditionalCall> Conditions;
    
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
        if (!OEIJsonUtils.ValidateObject(json, "Operator"))
        {
            throw new ArgumentException("ConditionalExpression is missing Operator");
        }

        return new ConditionalExpression
        {
            Conditions = json["Components"].Select(ConditionalCall.TryParse).ToList(),
            Operator = (json["Operator"] ?? 0).Value<ComparisonType>()
        };
    }
}