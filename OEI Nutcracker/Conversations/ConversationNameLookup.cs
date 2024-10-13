using Newtonsoft.Json;
using Newtonsoft.Json.Linq;

namespace OEI_Nutcracker;

public class ConversationNameLookup
{
    public Guid ID;
    public string Name;
    public string Filename;
    
    public static ConversationNameLookup TryParse(JObject json)
    {
        try
        {
            return new ConversationNameLookup
            {
                ID = Guid.Parse(json["ID"].Value<string>()),
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