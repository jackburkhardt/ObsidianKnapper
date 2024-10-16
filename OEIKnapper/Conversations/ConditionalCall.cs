using Newtonsoft.Json.Linq;

namespace OEIKnapper.Conversations;

public class ConditionalCall
{
    public static ConditionalCall TryParse(JToken json)
    {

        return new ConditionalCall();
    }
}