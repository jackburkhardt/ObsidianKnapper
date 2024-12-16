using Newtonsoft.Json;
using Newtonsoft.Json.Linq;

namespace OEIKnapper.Filesystem;

public class StringTableReader : FileReader
{
    public StringTableReader(string path)
    {
        _path = path;
    }

    public async Task<IList<StringTable>> Read()
    {
        try
        {
            var fileText = await File.ReadAllTextAsync(_path);
            var json = JObject.Parse(fileText);
            var tables = new List<StringTable>();

            foreach (var tableJson in json["StringTables"])
            {
                tables.Add(StringTable.TryParse(tableJson));
            }

            return tables;
        }
        catch (Exception e)
        {
            Console.WriteLine("Failed to parse string table file: " + _path);
            Console.WriteLine($"{e.Source} -> {e.Message}");
            return [];
        }
    }
}