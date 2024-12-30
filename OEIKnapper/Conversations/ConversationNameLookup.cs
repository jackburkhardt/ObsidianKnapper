using Newtonsoft.Json;
using Newtonsoft.Json.Linq;

namespace OEIKnapper.Conversations;

/// <summary>
/// Part of OEI's .conversationbundle which acts as a lookup table for conversation names.
/// </summary>
public class ConversationNameLookup : IBundleItem
{
    public Guid ID { get; set; }
    [JsonProperty(PropertyName = "Name")]
    public string Tag { get; set; }
    
    public string Filename;
    
    public static ConversationNameLookup TryParse(JToken json)
    {
        try
        {
            var filename = json["Filename"].Value<string>();
            var name = json["Name"] != null ? json["Name"].Value<string>() : Path.GetFileName(filename);
            return new ConversationNameLookup
            {
                ID = json["ID"].ToObject<Guid>(),
                Tag = name,
                Filename = filename
            };
        }
        catch (Exception)
        {
            throw new ArgumentException("Unable to parse ConversationNameLookup from: " + json.ToString(Formatting.None));
        }
    }
}