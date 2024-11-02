using System.IO;
using OEIKnapper.Conversations;
using OEIKnapper.Filesystem;
using OEIKnapper.Quests;
using OEIKnapperGUI;

namespace OEIKnapper;

public class Database
{
    public record OperationProgress(int total = 0, int current = 0, string message = "");
    public Bundle<GlobalVariable> GlobalVariables { get; private set; } = new();
    public Bundle<Quest> Quests { get; private set; } = [];
    public Bundle<ConversationNameLookup> ConvoLookup { get; private set; } = [];
    public Bundle<Speaker> Speakers { get; private set; } = [];
    public Dictionary<string, StringTable> StringTable { get; private set; } = new();
    
    
    public Database()
    {
        FileReader.OnFileParsedEvent += OnFileParsed;
        FileReader.OnFileParseFailedEvent += LogParseFailure;
    }
    
    public void LoadProject(string path)
    {
        
        var foundFiles = GameRummager.RummageForGameFiles(path);
        GameRummager.AssignFilesToReaders(foundFiles);
        GameRummager.LoadStringTable("enus");
        
        
    }
    
    private void OnFileParsed(object data, Type type, string path)
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
    
    private void LogParseFailure(string path, string message)
    {
        Console.WriteLine($"Failed to parse file: {path} - {message}");
    }
}