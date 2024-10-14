using Newtonsoft.Json;
using Newtonsoft.Json.Linq;

namespace OEIKnapper;

public class Conversation
{
    public Guid ID;
    public string Filename;
    public List<Guid> CharacterMappings;
    public List<Node> Nodes;
    
    public static Conversation TryParse(JToken json)
    {
        if (!JsonFieldValidate.ValidateObject(json, "ID", "Filename", "CharacterMappings", "Nodes"))
        {
            throw new ArgumentException("Unable to parse Conversation from: " + json.ToString(Formatting.None));
        }

        var nodes = json["Nodes"].Select(Node.TryParse).ToList();

        return new Conversation
        {
            ID = Guid.Parse(json["ID"].Value<string>()),
            Filename = json["Filename"].Value<string>(),
            CharacterMappings = json["CharacterMappings"].Select(x => Guid.Parse(x.Value<string>())).ToList(),
            Nodes = nodes
        };
    }
}