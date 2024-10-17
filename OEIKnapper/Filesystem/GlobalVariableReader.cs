using Newtonsoft.Json.Linq;
using OEIKnapper.Conversations;

namespace OEIKnapper.Filesystem;

public class GlobalVariableReader
{
    string _path;
    
    public GlobalVariableReader(string path)
    {
        _path = path;
    }

    public async Task<List<GlobalVariable>> Read()
    {
        var fileText = await File.ReadAllTextAsync(_path);
        var json = JObject.Parse(fileText);
        
        var variables = new List<GlobalVariable>();
        foreach (var varSet in json["GlobalVariablesSets"])
        {
            variables.AddRange(varSet["GlobalVariables"].Select(GlobalVariable.TryParse));
        }
        
        return variables;
    }
}