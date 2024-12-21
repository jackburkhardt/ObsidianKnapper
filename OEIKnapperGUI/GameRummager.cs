using System.IO;
using System.Windows;
using System.Windows.Interop;
using System.Windows.Media.Imaging;
using OEIKnapper;

namespace OEIKnapperGUI;

public class GameRummager
{
    public const string StringTableExt = "stringtablebundle";
    public const string ConversationBundleExt = "conversationbundle";
    public const string QuestExt = "questbundle";
    public const string ConversationExt = "conversationasset";
    public const string GlobalVariableExt = "globalvariablesbundle";
    public const string OAFExt = "oaf";

    private static string[] GameFileExtensions = new string[]
    {
        StringTableExt,
        ConversationExt,
        ConversationBundleExt,
        QuestExt,
        GlobalVariableExt,
        OAFExt
    };

    public static IList<GameExeData> RummageForGames()
    {
        string[] searchDirs = [];
        string[] gamePaths = [];
        List<GameExeData> foundGames = [];
        
        try
        {
            searchDirs = File.ReadAllLines(@"Config\searchdirs.txt");
            gamePaths = File.ReadAllLines(@"Config\gamepaths.txt");
        } catch (Exception e)
        {
            Console.WriteLine("GameRummager could not find required Config/searchdirs.txt or Config/gamepaths.txt files.");
            Console.WriteLine(e);
        }
        
        // ReSharper disable once LoopCanBeConvertedToQuery (for readability)
        foreach (var dir in searchDirs)
        {
            foreach (var path in gamePaths)
            {
                var fullPath = dir + path;
                if (File.Exists(fullPath))
                {
                    foundGames.Add(LoadProjectData(fullPath));
                }
            }
        }
            
        return foundGames;
    }
    
    public static GameExeData LoadProjectData(string path)
    {
        var icon = System.Drawing.Icon.ExtractAssociatedIcon(path);
        var iconBitmapImg = Imaging.CreateBitmapSourceFromHIcon(icon.Handle, Int32Rect.Empty, BitmapSizeOptions.FromEmptyOptions());
        var fname = path.Split('\\').Last();

        return new GameExeData(iconBitmapImg, fname, path);
    }

    public static void RummageForGameFiles(ref KnapperProject project)
    {
        var gamePath = project.GamePath[..project.GamePath.LastIndexOf('\\')];
        var foundFiles = Directory.EnumerateFiles(gamePath, "*.*", SearchOption.AllDirectories)
            .Where(file => GameFileExtensions.Any(ext => file.EndsWith(ext, StringComparison.OrdinalIgnoreCase))).ToArray();
        
        project.GameAssets = [];
        project.SupportedFeatures = [];
        
        foreach (var path in foundFiles)
        {
            switch (Path.GetExtension(path).TrimStart('.').ToLower())
            {
                case ConversationExt:
                    project.GameAssets.Add(new KnapperProject.GameAsset(path, false, ConversationExt));
                    break;
                case ConversationBundleExt:
                    project.GameAssets.Add(new KnapperProject.GameAsset(path, false, ConversationBundleExt));
                    if (!project.SupportedFeatures.Contains(KnapperFeature.ConvoEditor)) project.SupportedFeatures.Add(KnapperFeature.ConvoEditor);
                    break;
                case QuestExt:
                    project.GameAssets.Add(new KnapperProject.GameAsset(path, false, QuestExt));
                    if (!project.SupportedFeatures.Contains(KnapperFeature.QuestEditor)) project.SupportedFeatures.Add(KnapperFeature.QuestEditor);
                    break;
                case StringTableExt:
                    project.GameAssets.Add(new KnapperProject.GameAsset(path, false, StringTableExt));
                    if (!project.SupportedFeatures.Contains(KnapperFeature.StringTableEditor)) project.SupportedFeatures.Add(KnapperFeature.StringTableEditor);
                    break;
                case GlobalVariableExt:
                    project.GameAssets.Add(new KnapperProject.GameAsset(path, false, GlobalVariableExt));
                    if (!project.SupportedFeatures.Contains(KnapperFeature.GlobalVarEditor)) project.SupportedFeatures.Add(KnapperFeature.GlobalVarEditor);
                    break;
                case OAFExt:
                    project.GameAssets.Add(new KnapperProject.GameAsset(path, false, OAFExt));
                    if (!project.SupportedFeatures.Contains(KnapperFeature.OAFReader)) project.SupportedFeatures.Add(KnapperFeature.OAFReader);
                    break;
            }
        }
    }
} 

public struct GameExeData()
{
    public BitmapSource Icon { get; private set; }
    public string Name { get; private set; }
    public string Path { get; private set; }
    
    public GameExeData(BitmapSource icon, string name, string path) : this()
    {
        Icon = icon;
        Name = name;
        Path = path;
    }
};