using Newtonsoft.Json.Linq;

namespace OEIKnapper.Conversations;

public class ConditionalExpression : Conditional
{
    public ComparisonType Operator;
    public List<Conditional> Conditions;
    
    public static ConditionalExpression TryParse(JToken json)
    {
        if (!OEIJsonUtils.ValidateObject(json, "Operator"))
        {
            throw new ArgumentException("ConditionalExpression is missing Operator");
        }
        
        var expression = Conditional.TryParse(json) as ConditionalExpression;
        expression.Operator = (ComparisonType)(json["Operator"] ?? 0).Value<int>();

        return expression;
    }
}