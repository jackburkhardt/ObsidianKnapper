using Newtonsoft.Json;
using Newtonsoft.Json.Linq;

namespace OEIKnapper;

public class Speaker
{
    public Guid ID;
    public int DisplayNameStringTableID;
    public int DisplayNameID;
    public string ObjectName;

    public static Speaker TryParse(JToken json)
    {
        if (!OEIJsonUtils.ValidateObject(json, "ID", "DisplayNameStringTableID", "ObjectName"))
        {
            throw new ArgumentException("Unable to parse Speaker from: " + json.ToString(Formatting.None));
        }

        return new Speaker
        {
            ID = Guid.Parse(json["ID"].Value<string>()),
            DisplayNameStringTableID = json["DisplayNameStringTableID"].Value<int>(),
            DisplayNameID = json["DisplayNameID"]?.Value<int>() ?? -1,
            ObjectName = json["ObjectName"]?.Value<string>()
        };
    }
}