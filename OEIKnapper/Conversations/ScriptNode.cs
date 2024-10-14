using Newtonsoft.Json.Linq;

namespace OEIKnapper;

public class ScriptNode : Node
{
    public static ScriptNode TryParse(JToken json)
    {
        return new ScriptNode();
    }
}