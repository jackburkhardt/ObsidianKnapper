using Newtonsoft.Json.Linq;

namespace OEIKnapper.Filesystem;

public class ConversationReader
{
    string _path;
    
    public ConversationReader(string path)
    {
        _path = path;
    }

    public async Task<Conversation> Read()
    {
        var fileText = await File.ReadAllTextAsync(_path);
        var json = JObject.Parse(fileText);
        
        return Conversation.TryParse(json);
    }
}