using Newtonsoft.Json.Linq;
using OEIKnapper.Conversations;

namespace OEIKnapper.Filesystem;

public class GlobalVariableReader : FileReader
{

    public GlobalVariableReader(string path)
    {
        _path = path;
    }

    public async Task Read()
    {
        try {
            var fileText = await File.ReadAllTextAsync(_path);
            var json = JObject.Parse(fileText);
            
            var variables = new List<GlobalVariable>();
            foreach (var varSet in json["GlobalVariablesSets"])
            {
                variables.AddRange(varSet["GlobalVariables"].Select(GlobalVariable.TryParse));
            }
            
            RaiseFileParsedEvent(variables, variables.GetType(), _path);
        }
        catch (Exception e)
        {
            RaiseFileParseFailedEvent(_path, $"{e.Source} -> {e.Message}");
        }
    }
}