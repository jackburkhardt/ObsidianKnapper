using Newtonsoft.Json;
using Newtonsoft.Json.Linq;

namespace OEIKnapper.Conversations;

public class ConversationNameLookup
{
    public Guid ID;
    public string Name;
    public string Filename;
    
    public static ConversationNameLookup TryParse(JToken json)
    {
        try
        {
            return new ConversationNameLookup
            {
                ID = json["ID"].ToObject<Guid>(),
                Name = json["Name"].Value<string>(),
                Filename = json["Filename"].Value<string>()
            };
        }
        catch (Exception)
        {
            throw new ArgumentException("Unable to parse ConversationNameLookup from: " + json.ToString(Formatting.None));
        }
    }
}