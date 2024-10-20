using Newtonsoft.Json.Linq;
using OEIKnapper.Quests;

namespace OEIKnapper.Filesystem;

public class QuestBundleReader
{
    string _path;
    
    public QuestBundleReader(string path)
    {
        _path = path;
    }

    public async Task<List<Quest>> Read()
    {
        var fileText = await File.ReadAllTextAsync(_path);
        var json = JObject.Parse(fileText);

        return json["Quests"].Select(Quest.TryParse).ToList();
    }
}