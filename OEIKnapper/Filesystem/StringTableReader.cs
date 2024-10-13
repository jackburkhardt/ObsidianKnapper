using Newtonsoft.Json;
using Newtonsoft.Json.Linq;

namespace OEIKnapper.Filesystem;

public class StringTableReader
{
    string _path;
    
    public StringTableReader(string path)
    {
        _path = path;
    }

    public async Task<List<StringTable>> Read()
    {
        var fileText = await File.ReadAllTextAsync(_path);
        var json = JObject.Parse(fileText);
        var tables = new List<StringTable>();

        foreach (var tableJson in json["StringTables"] as JArray)
        {
            tables.Add(StringTable.TryParse(tableJson));
        }
        
        return tables;
    }
}