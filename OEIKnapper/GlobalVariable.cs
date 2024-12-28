using Newtonsoft.Json;
using Newtonsoft.Json.Linq;

namespace OEIKnapper;

public class GlobalVariable : IBundleItem
{
    public Guid ID { get; set; }
    public string Tag { get; set; } // "b" prefix is bool, otherwise int...?
    [JsonProperty("VariableType")]
    public GlobalVarDataType Type { get; set; } // this is not a data type in the OEI schema, but I can't figure it out.
    public string InitialValue { get; set; }
    
    public static GlobalVariable TryParse(JToken json)
    {
        if (!OEIJsonUtils.ValidateObject(json, "ID", "Tag"))
        {
            throw new ArgumentException("Unable to parse GlobalVariable from: " + json.ToString(Formatting.None));
        }

        GlobalVarDataType varType = json["Tag"].Value<string>().StartsWith('b') ? GlobalVarDataType.Boolean : GlobalVarDataType.Integer;
        
        return new GlobalVariable
        {
            ID = json["ID"].ToObject<Guid>(),
            Tag = json["Tag"].Value<string>(),
            Type = varType,
            InitialValue = json["InitialValue"]?.Value<string>() ?? ""
        };
    }
}

public enum GlobalVarDataType
{
    Boolean,
    String,
    Integer,
    Float
}