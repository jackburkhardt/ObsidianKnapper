using Newtonsoft.Json.Linq;

namespace OEIKnapper.Conversations;

/// <summary>
/// Node which contains scripts to be executed.
/// </summary>
public class ScriptNode : Node
{
    public static ScriptNode TryParse(JToken json)
    {
        return new ScriptNode();
    }
}