using Newtonsoft.Json.Linq;
using OEIKnapper.Conversations;

namespace OEIKnapper.Filesystem;

public class ConversationReader : FileReader
{
    public ConversationReader(string path)
    {
        _path = path;
    }

    public async Task<Conversation> Read()
    {
        try {
            var fileText = await File.ReadAllTextAsync(_path);
            var json = JObject.Parse(fileText);
            var parsedConvo = Conversation.TryParse(json);

            return parsedConvo;
        }
        catch (Exception e)
        {
            Console.WriteLine("Failed to parse conversation file: " + _path);
            Console.WriteLine($"{e.Source} -> {e.Message}");
            return new Conversation();
        }
    }
}