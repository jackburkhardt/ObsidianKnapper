using Newtonsoft.Json.Linq;
using OEIKnapper.Conversations;

namespace OEIKnapper.Quests;

public class Quest
{
    public Guid ID;
    public string Filename;
    public int TotalExperienceWeight;
    public List<Node> Nodes;
    public List<string> ExtendedProperties;

    public static Quest TryParse(JToken json)
    {
        return new Quest
        {
            ID = json["ID"].ToObject<Guid>(),
            Filename = json["Filename"].Value<string>(),
            TotalExperienceWeight = json["TotalExperienceWeight"].Value<int>(),
            Nodes = json["Nodes"].Select(Node.TryParse).ToList(),
            ExtendedProperties = json["ClassExtender"]["ExtendedProperties"].ToObject<List<string>>()
        };
    }
}