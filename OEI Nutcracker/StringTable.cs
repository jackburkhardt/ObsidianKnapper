using Newtonsoft.Json;
using Newtonsoft.Json.Linq;

namespace OEI_Nutcracker;

public class StringTable
{
    public string Name;
    public List<String> Strings;
    
    public static StringTable TryParse(JToken json)
    {
        if (!JsonFieldValidate.ValidateObject(json, "Name", "Entries"))
        {
            throw new ArgumentException("Unable to parse StringTable from: " + json.ToString(Formatting.None));
        }
        
        return new StringTable
        {
            Name = json["Name"].Value<string>(),
            Strings = json["Entries"].Select(String.TryParse).ToList()
        };
    }
}

public class String
{
    public int ID;
    public string DefaultText;

    public static String TryParse(JToken json)
    {
        if (!JsonFieldValidate.ValidateObject(json, "ID", "DefaultText"))
        {
            throw new ArgumentException("Unable to parse String from: " + json.ToString(Formatting.None));
        }
        
        return new String
        {
            ID = json["ID"].Value<int>(),
            DefaultText = json["DefaultText"].Value<string>()
        };
    }
}