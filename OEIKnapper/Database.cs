using OEIKnapper.Conversations;
using OEIKnapper.Filesystem;
using OEIKnapper.Quests;

namespace OEIKnapper;

public class Database
{
    public record OperationProgress(int total = 0, int current = 0, string message = "");
    public Dictionary<Guid, GlobalVariable> GlobalVariables { get; private set; } = new();
    public Bundle<Quest> Quests { get; private set; } = new();
    public Bundle<ConversationNameLookup> ConvoLookup { get; private set; } = new();
    public Dictionary<string, StringTable> StringTable { get; private set; } = new();
    
    public static void LoadProject(string path)
    {
        var foundFiles = GameRummager.RummageForGameFiles(path);
        GameRummager.AssignFilesToReaders(foundFiles);
        
        
    }
}