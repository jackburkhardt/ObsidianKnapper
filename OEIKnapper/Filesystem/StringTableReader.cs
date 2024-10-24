using Newtonsoft.Json;
using Newtonsoft.Json.Linq;

namespace OEIKnapper.Filesystem;

public class StringTableReader : FileReader
{
  
    public StringTableReader(string path)
    {
        _path = path;
    }

    public async Task Read()
    {
        var fileText = await File.ReadAllTextAsync(_path);
        var json = JObject.Parse(fileText);
        var tables = new List<StringTable>();

        foreach (var tableJson in json["StringTables"] as JArray)
        {
            tables.Add(StringTable.TryParse(tableJson));
        }
        
        RaiseFileParsedEvent(tables, tables.GetType(), _path);
    }
}