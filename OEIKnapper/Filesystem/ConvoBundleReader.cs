using Newtonsoft.Json.Linq;
using OEIKnapper.Conversations;

namespace OEIKnapper.Filesystem;

public class ConvoBundleReader
{
    string _path;
    
    public ConvoBundleReader(string path)
    {
        _path = path;
    }

    public async Task<(List<Speaker>, List<ConversationNameLookup>)> Read(AsyncCallback callback = null)
    {
        var fileText = await File.ReadAllTextAsync(_path);
        var json = JObject.Parse(fileText);
        var speakers = new List<Speaker>();

        foreach (var tableJson in json["Speakers"] as JArray)
        {
            speakers.Add(Speaker.TryParse(tableJson));
        }
        
        var conversations = new List<ConversationNameLookup>();
        
        foreach (var tableJson in json["ConversationNameLookup"] as JArray)
        {
            conversations.Add(ConversationNameLookup.TryParse(tableJson));
        }
        
        return (speakers, conversations);
    }
}