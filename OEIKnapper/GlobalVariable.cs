using Newtonsoft.Json;
using Newtonsoft.Json.Linq;

namespace OEIKnapper;

public struct GlobalVariable
{
    public Guid ID;
    public string Tag; // "b" prefix is bool, otherwise int?
    public int Type; // todo: what is this? not a datatype it seems
    public string InitialValue;
    
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
            Type = json["VariableType"].Value<int>(),
            InitialValue = json["InitialValue"]?.Value<string>() ?? ""
        };
    }
}