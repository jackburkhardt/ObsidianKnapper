using Newtonsoft.Json;
using Newtonsoft.Json.Linq;

namespace OEIKnapper.Conversations;

public class ConversationNameLookup : IBundleItem
{
    public Guid ID { get; set; }
    public string Name;
    [JsonProperty(PropertyName = "Filename")]
    public string Tag { get; set; }
    
    public static ConversationNameLookup TryParse(JToken json)
    {
        try
        {
            return new ConversationNameLookup
            {
                ID = json["ID"].ToObject<Guid>(),
                Name = json["Name"].Value<string>(),
                Tag = json["Filename"].Value<string>()
            };
        }
        catch (Exception)
        {
            throw new ArgumentException("Unable to parse ConversationNameLookup from: " + json.ToString(Formatting.None));
        }
    }
}