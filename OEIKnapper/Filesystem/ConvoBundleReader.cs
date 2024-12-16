using Newtonsoft.Json.Linq;
using OEIKnapper.Conversations;

namespace OEIKnapper.Filesystem;

public class ConvoBundleReader : FileReader
{
    public ConvoBundleReader(string path)
    {
        _path = path;
    }

    public async Task<(IList<Speaker> speakers, IList<ConversationNameLookup> convos)> Read()
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
            return result;
        }
        catch (Exception e)
        {
            Console.WriteLine("Failed to parse bundle file: " + _path);
            Console.WriteLine($"{e.Source} -> {e.Message}");
            return new();
        }
    }
}