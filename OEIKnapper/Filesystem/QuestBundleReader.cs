using Newtonsoft.Json.Linq;
using OEIKnapper.Quests;

namespace OEIKnapper.Filesystem;

public class QuestBundleReader : FileReader
{

    public QuestBundleReader(string path)
    {
        _path = path;
    }

    public async Task<IList<Quest>> Read()
    {
        try {
            var fileText = await File.ReadAllTextAsync(_path);
            var json = JObject.Parse(fileText);
            var questList = json["Quests"].Select(Quest.TryParse).ToList();
            
            return questList;
        }
        catch (Exception e)
        {
            Console.WriteLine("Failed to parse quest file: " + _path);
            Console.WriteLine($"{e.Source} -> {e.Message}");
            return [];
        }
    }
}