using Newtonsoft.Json.Linq;
using OEIKnapper.Conversations;

namespace OEIKnapper.Filesystem;

public class ConversationReader : FileReader
{
    public ConversationReader(string path)
    {
        _path = path;
    }

    public async Task Read()
    {
        var fileText = await File.ReadAllTextAsync(_path);
        var json = JObject.Parse(fileText);
        var parsedConvo =  Conversation.TryParse(json);
        
        RaiseFileParsedEvent(parsedConvo, parsedConvo.GetType(), _path);
    }
}