using Newtonsoft.Json.Linq;
using OEIKnapper.Quests;

namespace OEIKnapper.Filesystem;

public class QuestBundleReader : FileReader
{

    public QuestBundleReader(string path)
    {
        _path = path;
    }

    public async Task Read()
    {
        try {
            var fileText = await File.ReadAllTextAsync(_path);
            var json = JObject.Parse(fileText);
            var questList = json["Quests"].Select(Quest.TryParse).ToList();
            
            RaiseFileParsedEvent(questList, questList.GetType(), _path);
        }
        catch (Exception e)
        {
            RaiseFileParseFailedEvent(_path, e.Message);
        }
    }
}