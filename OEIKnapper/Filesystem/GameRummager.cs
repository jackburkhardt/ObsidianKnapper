using System.Collections;
using OEIKnapper.Conversations;

namespace OEIKnapper.Filesystem;

public class GameRummager
{
    public const string StringTableExt = "stringtablebundle";
    private const string ConversationBundleExt = "conversationbundle";
    private const string QuestExt = "questbundle";
    private const string ConversationExt = "conversationasset";
    private const string GlobalVariableExt = "globalvariablesbundle";

    private static string[] GameFileExtensions = new string[]
    {
        StringTableExt,
        ConversationBundleExt,
        QuestExt,
        GlobalVariableExt
    };

    public delegate void ReadComplete();

    public static IEnumerable<string> RummageForGameFiles(string gamePath)
    {
        // chop the file off the path
        gamePath = gamePath[..gamePath.LastIndexOf('\\')];
        var foundFiles = Directory.EnumerateFiles(gamePath, "*.*", SearchOption.AllDirectories)
            .Where(file => GameFileExtensions.Any(ext => file.EndsWith(ext, StringComparison.OrdinalIgnoreCase)));
        return foundFiles;
    }

    public static void AssignFilesToReaders(IEnumerable<string> paths)
    {
        var tasks = new List<Task>();

        foreach (var path in paths)
        {
            switch (path.Split('.')[1])
            {
                case ConversationBundleExt:
                    var convoReader = new ConvoBundleReader(path);
                    convoReader.OnFileParsedEvent += Database.OnFileParsed;
                    tasks.Add(convoReader.Read());
                    break;
                case QuestExt:
                    var questReader = new QuestBundleReader(path);
                    questReader.OnFileParsedEvent += Database.OnFileParsed;
                    tasks.Add(questReader.Read());
                    break;
                case GlobalVariableExt:
                    var globalVarReader = new GlobalVariableReader(path);
                    globalVarReader.OnFileParsedEvent += Database.OnFileParsed;
                    tasks.Add(globalVarReader.Read());
                    break;
                case ConversationExt:
                    break;
                default:
                    Console.WriteLine($"File not assigned to a reader: {path}");
                    break;
            }
        }

        Task.WhenAll(tasks);
    }
} 