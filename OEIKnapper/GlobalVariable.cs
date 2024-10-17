using Newtonsoft.Json;
using Newtonsoft.Json.Linq;

namespace OEIKnapper;

public struct GlobalVariable
{
    public Guid ID;
    public string Tag;
    public VariableType Type;
    public string InitialValue;
    
    
    public enum VariableType
    {
        String,
        Integer,
        Boolean,
        Float
    }
    
    public static GlobalVariable TryParse(JToken json)
    {
        if (!OEIJsonUtils.ValidateObject(json, "ID", "Tag"))
        {
            throw new ArgumentException("Unable to parse GlobalVariable from: " + json.ToString(Formatting.None));
        }
        
        return new GlobalVariable
        {
            ID = json["ID"].ToObject<Guid>(),
            Tag = json["Tag"].Value<string>(),
            Type = json["VariableType"] != null ? (VariableType)json["VariableType"].Value<int>() : VariableType.Boolean,
            InitialValue = json["InitialValue"]?.Value<string>() ?? ""
        };
    }
}