using System.Xml;
using System.Xml.Linq;
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
            bool isJson = fileText.StartsWith('{');
            var tables = new List<StringTable>();

            if (isJson)
            {
                var json = JObject.Parse(fileText);
                if (json["StringTables"] != null)
                {
                    foreach (var table in json["StringTables"])
                    {
                        tables.Add(StringTable.TryParse(table));
                    }
                }
            }
            else
            {
                if (!fileText.StartsWith('<'))
                {
                    throw new ArgumentException("StringTable file is not in a recognized format (JSON or XML): " + _path);
                }
                var xmlDoc = new XmlDocument();
                xmlDoc.LoadXml(fileText);
                var tabelXml = XElement.Parse(xmlDoc.InnerXml); 
                tables.Add(StringTable.TryParseXML(tabelXml));
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