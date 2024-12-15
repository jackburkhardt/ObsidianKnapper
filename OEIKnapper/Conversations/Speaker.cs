using Newtonsoft.Json;
using Newtonsoft.Json.Linq;

namespace OEIKnapper.Conversations;

public class Speaker : IBundleItem
{
    public Guid ID { get; set; }
    public int DisplayNameStringTableID;
    public int DisplayNameID;
    [JsonProperty(PropertyName = "ObjectName")]
    public string Tag { get; set; } = "";

    public static Speaker TryParse(JToken json)
    {
        if (!OEIJsonUtils.ValidateObject(json, "ID", "DisplayNameStringTableID", "ObjectName"))
        {
            throw new ArgumentException("Unable to parse Speaker from: " + json.ToString(Formatting.None));
        }

        return new Speaker
        {
            ID = json["ID"].ToObject<Guid>(),
            DisplayNameStringTableID = json["DisplayNameStringTableID"].Value<int>(),
            DisplayNameID = json["DisplayNameID"]?.Value<int>() ?? -1,
            Tag = json["ObjectName"]?.Value<string>()
        };
    }
}