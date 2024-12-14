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
        try {
            var fileText = await File.ReadAllTextAsync(_path);
            var json = JObject.Parse(fileText);
            var parsedConvo = Conversation.TryParse(json);
            
            RaiseFileParsedEvent(parsedConvo, parsedConvo.GetType(), _path);
        }
        catch (Exception e)
        {
            RaiseFileParseFailedEvent(_path, $"{e.Source} -> {e.Message}");
        }
    }
}