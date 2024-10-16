using Newtonsoft.Json.Linq;

namespace OEIKnapper.Conversations;

public class ConditionalExpression
{
    public static ConditionalExpression TryParse(JToken json)
    {

        return new ConditionalExpression();
    }
}