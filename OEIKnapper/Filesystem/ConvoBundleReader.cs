using Newtonsoft.Json.Linq;
using OEIKnapper.Conversations;

namespace OEIKnapper.Filesystem;

public class ConvoBundleReader : FileReader
{
    public ConvoBundleReader(string path)
    {
        _path = path;
    }

    public async Task Read()
    { 
        try {
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
            
            var result = (speakers, conversations);
            RaiseFileParsedEvent(result, result.GetType(), _path);
        }
        catch (Exception e)
        {
            RaiseFileParseFailedEvent(_path, e.Message);
        }
    }
}