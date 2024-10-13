using System.Collections;

namespace OEI_Nutcracker.Filesystem;

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
                    Task.Run(() =>
                    {
                        var stringTableReader = new StringTableReader(path);
                        var tables = stringTableReader.Read().Result;
                        Console.WriteLine("Table parsed.");
                    });
                    break;
                
                default:
                    Console.WriteLine($"File not assigned to a reader: {path}");
                    break; 
            }
        }
    }
}