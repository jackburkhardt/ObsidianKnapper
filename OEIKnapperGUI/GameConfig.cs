using System.Dynamic;
using System.IO;
using Newtonsoft.Json;
using Newtonsoft.Json.Converters;

namespace OEIKnapperGUI;

public class GameConfig
{
    public static dynamic Current;

    public static void LoadConfig(string gameName)
    {
        if (File.Exists(@$"Config\Games\{gameName}.json"))
        {
            var fileText = File.ReadAllText(@$"Config\Games\{gameName}.json");
            var converter = new ExpandoObjectConverter();
            Current = JsonConvert.DeserializeObject<ExpandoObject>(fileText, converter);
        }
        else
        {
            var defaultText = File.ReadAllText(@$"Config\Games\DEFAULT.json");
            var converter = new ExpandoObjectConverter();
            Current = JsonConvert.DeserializeObject<ExpandoObject>(defaultText, converter);
        }
    }
}