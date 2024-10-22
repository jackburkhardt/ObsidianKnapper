using Newtonsoft.Json;
using Newtonsoft.Json.Linq;

namespace OEIKnapper.Conversations;

public class ConditionalExpression : Conditional
{
    [JsonProperty(DefaultValueHandling = DefaultValueHandling.Ignore)]
    public ComparisonType Operator;
    [JsonProperty(PropertyName = "Components")]
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