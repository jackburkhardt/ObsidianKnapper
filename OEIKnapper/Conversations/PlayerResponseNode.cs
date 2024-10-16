using Newtonsoft.Json.Linq;

namespace OEIKnapper;

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