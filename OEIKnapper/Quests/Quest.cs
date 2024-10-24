using Newtonsoft.Json;
using Newtonsoft.Json.Linq;
using OEIKnapper.Conversations;

namespace OEIKnapper.Quests;

public class Quest : IBundleItem
{
    public Guid ID { get; set; }
    [JsonProperty(PropertyName = "Filename")]
    public string Tag { get; set; }
    
    public int TotalExperienceWeight;
    public List<Node> Nodes;
    public List<string> ExtendedProperties;
    
    

    public static Quest TryParse(JToken json)
    {
        return new Quest
        {
            ID = json["ID"].ToObject<Guid>(),
            Tag = json["Filename"].Value<string>(),
            TotalExperienceWeight = json["TotalExperienceWeight"].Value<int>(),
            Nodes = json["Nodes"].Select(Node.TryParse).ToList(),
            ExtendedProperties = json["ClassExtender"]["ExtendedProperties"].ToObject<List<string>>()
        };
    }
}