using Newtonsoft.Json.Linq;
using OEIKnapper.Conversations;

namespace OEIKnapper.Filesystem;

public class GlobalVariableReader : FileReader
{

    public GlobalVariableReader(string path)
    {
        _path = path;
    }

    public async Task<IList<GlobalVariable>> Read()
    {
        try {
            var fileText = await File.ReadAllTextAsync(_path);
            var json = JObject.Parse(fileText);
            
            var variables = new List<GlobalVariable>();
            foreach (var varSet in json["GlobalVariablesSets"])
            {
                variables.AddRange(varSet["GlobalVariables"].Select(GlobalVariable.TryParse));
            }

            return variables;
        }
        catch (Exception e)
        {
            Console.WriteLine("Failed to parse global variable file: " + _path);
            Console.WriteLine($"{e.Source} -> {e.Message}");
            return [];
        }
    }
}