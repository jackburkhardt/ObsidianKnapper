using Newtonsoft.Json;
using Newtonsoft.Json.Linq;

namespace OEI_Nutcracker;

public class Speaker
{
    public Guid ID;
    public int DisplayNameStringTableID;
    public int DisplayNameID;

    public static Speaker TryParse(JObject json)
    {
        if (json["ID"] == null || json["DisplayNameStringTableID"] == null || json["DisplayNameID"] == null)
        {
            throw new ArgumentException("Unable to parse Speaker from: " + json.ToString(Formatting.None));
        }

        return new Speaker
        {
            ID = json["ID"].Value<Guid>(),
            DisplayNameStringTableID = json["DisplayNameStringTableID"].Value<int>(),
            DisplayNameID = json["DisplayNameID"].Value<int>()
        };
    }
}