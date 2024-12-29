using System.IO;
using System.Windows;
using System.Windows.Interop;
using System.Windows.Media.Imaging;
using OEIKnapper;

namespace OEIKnapperGUI;

public class GameRummager
{
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
        var fname = Path.GetFileNameWithoutExtension(path);

        return new GameExeData(iconBitmapImg, fname, path);
    }
    
    public static void RummageForGameFiles(KnapperProject project)
    {
        string[] ExtensionsToSearch =
        [
            GameConfig.Current.StringTableExtension,
            GameConfig.Current.ConversationManifestExtension,
            GameConfig.Current.ConversationExtension,
            GameConfig.Current.QuestExtension,
            GameConfig.Current.GlobalVariableExtension,
            GameConfig.Current.OAFExtension
        ];
    
        var gamePath = project.GamePath[..project.GamePath.LastIndexOf('\\')];
        var foundFiles = Directory.EnumerateFiles(gamePath, "*.*", SearchOption.AllDirectories)
            .Where(file => ExtensionsToSearch.Any(ext => file.EndsWith(ext, StringComparison.OrdinalIgnoreCase))).ToArray();
        
        project.GameAssets = [];
        project.SupportedFeatures = [];
        
        foreach (var path in foundFiles)
        {
            string gameFileExt = Path.GetExtension(path).TrimStart('.').ToLower();
            
            if (gameFileExt == GameConfig.Current.StringTableExtension)
            {
                project.GameAssets.Add(new KnapperProject.GameAsset(path, false, GameConfig.Current.StringTableExtension));
                if (!project.SupportedFeatures.Contains(KnapperFeature.StringTableEditor)) project.SupportedFeatures.Add(KnapperFeature.StringTableEditor);
            }
            else if (gameFileExt == GameConfig.Current.ConversationManifestExtension)
            {
                project.GameAssets.Add(new KnapperProject.GameAsset(path, false, GameConfig.Current.ConversationManifestExtension));
                if (!project.SupportedFeatures.Contains(KnapperFeature.ConvoEditor)) project.SupportedFeatures.Add(KnapperFeature.ConvoEditor);
            }
            else if (gameFileExt == GameConfig.Current.ConversationExtension)
            {
                project.GameAssets.Add(new KnapperProject.GameAsset(path, false, GameConfig.Current.ConversationExtension));
            }
            else if (gameFileExt == GameConfig.Current.QuestExtension)
            {
                project.GameAssets.Add(new KnapperProject.GameAsset(path, false, GameConfig.Current.QuestExtension));
                if (!project.SupportedFeatures.Contains(KnapperFeature.QuestEditor)) project.SupportedFeatures.Add(KnapperFeature.QuestEditor);
            }
            else if (gameFileExt == GameConfig.Current.GlobalVariableExtension)
            {
                project.GameAssets.Add(new KnapperProject.GameAsset(path, false, GameConfig.Current.GlobalVariableExtension));
                if (!project.SupportedFeatures.Contains(KnapperFeature.GlobalVarEditor)) project.SupportedFeatures.Add(KnapperFeature.GlobalVarEditor);
            }
            else if (gameFileExt == GameConfig.Current.OAFExtension)
            {
                project.GameAssets.Add(new KnapperProject.GameAsset(path, false, GameConfig.Current.OAFExtension));
                if (!project.SupportedFeatures.Contains(KnapperFeature.OAFReader)) project.SupportedFeatures.Add(KnapperFeature.OAFReader);
            }
        }
    }

    public static void RummageForLocales(KnapperProject project)
    {
        // look for "localized" folder
        var directories = Directory.GetDirectories(project.Path, "localized", SearchOption.AllDirectories);
        if (directories.Length == 0)
        {
            Console.WriteLine("No localized folder found.");
            return;
        }
        
        var localeDirs = Directory.GetDirectories(directories[0]);
        project.AvailableLocales = localeDirs.Select(dir => dir.Split('\\').Last()).ToArray();
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