using Newtonsoft.Json;
using Newtonsoft.Json.Linq;
using OEIKnapper.Conversations;

namespace OEIKnapper;

/// <summary>
/// Collection of localized strings used in a <see cref="Conversation"/>.
/// </summary>
public class StringTable
{
    public string Name = "DEFAULT";
    public List<String> Strings = [];
    
    public static StringTable TryParse(JToken json)
    {
        if (!OEIJsonUtils.ValidateObject(json, "Name", "Entries"))
        {
            throw new ArgumentException("Unable to parse StringTable from: " + json.ToString(Formatting.None));
        }
        
        return new StringTable
        {
            Name = json["Name"].Value<string>(),
            Strings = json["Entries"].Select(String.TryParse).ToList()
        };
    }

    // opting to use this instead of a dictionary for better serialized schema compatibility
    public string this[int i]
    {
        get => Strings.FirstOrDefault(s => s.ID == i)?.DefaultText ?? "";
        set {
            if (Strings.FirstOrDefault(s => s.ID == i) != null)
            {
                Strings[i].DefaultText = value;
            }
            else
            {
                Strings.Add(new String { ID = i, DefaultText = value });
            }
        }
    }
}

/// <summary>
/// A localized string. Used as part of a <see cref="StringTable"/>.
/// </summary>
public class String
{
    public int ID { get; set; } = -1;
    public string DefaultText { get; set; } = "";

    public static String TryParse(JToken json)
    {
        if (!OEIJsonUtils.ValidateObject(json, "ID", "DefaultText"))
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