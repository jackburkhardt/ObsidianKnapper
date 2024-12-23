using Newtonsoft.Json.Linq;

namespace OEIKnapper.Conversations;

/// <summary>
/// Node describing a selectable player response.
/// </summary>
public class PlayerResponseNode : Node
{
    public int Persistence;
    
    public static PlayerResponseNode TryParse(JToken json)
    {
        return new PlayerResponseNode
        {
            Persistence = json["Persistence"].Value<int>()
        };
    }
}