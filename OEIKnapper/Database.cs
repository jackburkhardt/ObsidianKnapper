using System.IO;
using OEIKnapper;
using OEIKnapper.Conversations;
using OEIKnapper.Filesystem;
using OEIKnapper.Quests;

namespace OEIKnapper;

public class Database
{
    private const string baseGamePath = @"C:\Program Files (x86)\Steam\steamapps\common\Pentiment\Pentiment_Data\StreamingAssets";

    public record OperationProgress(int total = 0, int current = 0, string message = "");
    public static Bundle<GlobalVariable> GlobalVariables { get; private set; } = new();
    public static Bundle<Quest> Quests { get; private set; } = [];
    public static Bundle<ConversationNameLookup> ConvoLookup { get; private set; } = [];
    public static Bundle<Speaker> Speakers { get; private set; } = [];
    public static Dictionary<string, StringTable> StringTable { get; private set; } = new();
    
    public static void LoadProject(string path)
    {
        
        var foundFiles = GameRummager.RummageForGameFiles(path);
        GameRummager.AssignFilesToReaders(foundFiles);
    }
    
    public static void OnFileParsed(object data, Type type, string path)
    {
        Console.WriteLine($"Parsed file: {Path.GetFileName(path)}");
        switch (data)
        {
            case (List<Speaker> speakers, List<ConversationNameLookup> convos):
                ConvoLookup.AddRange(convos);
                Speakers.AddRange(speakers);
                break;
            case List<GlobalVariable> vars:
                GlobalVariables.AddRange(vars);
                break;
            case List<Quest> quests:
                Quests.AddRange(quests);
                break;
            case List<StringTable> stringTables:
                foreach (var table in stringTables)
                {
                    StringTable.Add(table.Name, table);
                }
                break;
            default:
                Console.WriteLine($"Unknown type: {type}");
                break;
        }
    }
    
    private static void LogParseFailure(string path, string message)
    {
        Console.WriteLine($"Failed to parse file: {path} - {message}");
    }
    
    public static void LoadConversation(string relativePath, Action<Conversation> callback)
    {
        var convoReader = new ConversationReader($"{baseGamePath}/design/conversations/{relativePath}");
        convoReader.OnFileParsedEvent += (data, type, path) =>
        {
            if (data is Conversation convo)
            {
                callback(convo);
            }
        };
        
        convoReader.OnFileParseFailedEvent += LogParseFailure;
        Task.Run(() => convoReader.Read());
    }
    
    public static void LoadStringTable(string locale, Action<List<StringTable>> callback)
    {
        var stringTableReader = new StringTableReader($"{baseGamePath}/localized/{locale}/text/text_{locale}.{GameRummager.StringTableExt}");
        stringTableReader.OnFileParsedEvent += (data, type, path) =>
        {
            if (data is List<StringTable> table)
            {
                callback(table);
            }
        };
        
        stringTableReader.OnFileParseFailedEvent += LogParseFailure;
        Task.Run(() => stringTableReader.Read());
    }
}