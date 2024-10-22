using System.Collections;

namespace OEIKnapper.Filesystem;

public class GameRummager
{
    private const string StringTableExt = ".stringtablebundle";
    private const string ConversationBundleExt = ".conversationbundle";
    private const string QuestExt = ".questbundle";
    private const string ConversationExt = ".conversationasset";
    private const string GlobalVariableExt = ".globalvariablesbundle";
    private static string[] GameFileExtensions = new string[]
    {
        StringTableExt,
        ConversationBundleExt,
        QuestExt,
        ConversationExt,
        GlobalVariableExt
    };
    public delegate void ReadComplete();
    
    public static IEnumerable<string> RummageForGameFiles(string gamePath)
    {
        var foundFiles = Directory.EnumerateFiles(gamePath, "*.*", SearchOption.AllDirectories)
            .Where(file => GameFileExtensions.Any(ext => file.EndsWith(ext, StringComparison.OrdinalIgnoreCase)));
        return foundFiles;
    }

    public static void AssignFilesToReaders(IEnumerable<string> paths)
    {
        
        foreach (var path in paths)
        {
            switch (path.Split('.')[1])
            {
                case StringTableExt:
                    var stringTableReader = new StringTableReader(path);
                   // tasks.Add(stringTableReader.Read());
                    break;
                case ConversationBundleExt:
                    var convoReader = new ConvoBundleReader(path);
                   // tasks.Add(convoReader.Read());
                    break;
                case QuestExt:
                    var questReader = new QuestBundleReader(path);
                    Task.Run(() => questReader.Read());
                    break;
                case GlobalVariableExt:
                    var globalVarReader = new GlobalVariableReader(path);
                    globalVarReader.Read();
                    break;
                default:
                    Console.WriteLine($"File not assigned to a reader: {path}");
                    break; 
            }
        }
        
    }
}