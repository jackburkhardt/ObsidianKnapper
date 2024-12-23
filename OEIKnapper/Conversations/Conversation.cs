using Newtonsoft.Json;
using Newtonsoft.Json.Linq;

namespace OEIKnapper.Conversations;

/// <summary>
/// Container for parsed Conversation (.conversationasset) data.
/// </summary>
public class Conversation : IBundleItem  
{
    public Guid ID { get; set; }
    [JsonProperty(PropertyName = "Filename")]
    public string Tag { get; set; }
    public List<Guid> CharacterMappings;
    public List<Node> Nodes = [];
    
    public static Conversation TryParse(JToken json)
    {
        if (!OEIJsonUtils.ValidateObject(json, "ID", "Filename", "CharacterMappings", "Nodes"))
        {
            throw new ArgumentException("Unable to parse Conversation from: " + json.ToString(Formatting.None));
        }

        var nodes = json["Nodes"].Select(Node.TryParse).ToList();

        return new Conversation
        {
            ID = json["ID"].ToObject<Guid>(),
            Tag = json["Filename"].Value<string>(),
            //CharacterMappings = json["CharacterMappings"].Select(x => Guid.Parse(x.Value<string>())).ToList(),
            Nodes = nodes
        };
    }
}