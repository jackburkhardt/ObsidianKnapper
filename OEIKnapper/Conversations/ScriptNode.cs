using Newtonsoft.Json.Linq;

namespace OEIKnapper.Conversations;

public class ScriptNode : Node
{
    public static ScriptNode TryParse(JToken json)
    {
        return new ScriptNode();
    }
}