using System.IO;
using Newtonsoft.Json;
using OEIKnapper;
using OEIKnapper.Conversations;
using OEIKnapper.Filesystem;
using OEIKnapper.Quests;

namespace OEIKnapperGUI;

public static class Database
{
    public class ProgressReport
    {
        public int Completed { get; set; }
        public int Total { get; set; }
        public string CurrentTask { get; set; }
    }

    public static KnapperProject CurrentProject;
    public static Bundle<GlobalVariable> GlobalVariables { get; private set; } = new();
    public static Bundle<Quest> Quests { get; private set; } = [];
    public static Bundle<ConversationNameLookup> ConvoLookup { get; private set; } = [];
    public static Bundle<Speaker> Speakers { get; private set; } = [];
    public static Dictionary<string, StringTable> StringTable { get; private set; } = new();

    public static async Task FindOrCreateProjectAsync(string gameContainingPath, string gameExePath)
    {
        var gameName = Path.GetFileNameWithoutExtension(gameExePath);
        GameConfig.LoadConfig(gameName);
        
        if (File.Exists($@"{gameContainingPath}\KnapperProject.cfg"))
        {
            return;
        }
        
        var newProject = new KnapperProject
        {
            Name = gameName,
            Path = gameContainingPath,
            GamePath = gameExePath,
            UsesGameConfig = gameName,
            SelectedLocale = GameConfig.Current.DefaultLocale,
        };
        
        newProject.SearchForGameAssets();
    }
    
    public static async Task LoadProjectAsync(string gameContainingPath, IProgress<ProgressReport> progress)
    {
        var projectLoadTasks = new List<(string path, Task task)>();

        var loadedProjectText = await File.ReadAllTextAsync(gameContainingPath + "\\KnapperProject.cfg");
        var loadedProjectFile = JsonConvert.DeserializeObject<KnapperProject>(loadedProjectText);
        if (loadedProjectFile == null)
        {
            throw new Exception("Database: Failed to load project file");
        }
        
        CurrentProject = loadedProjectFile;
        if (CurrentProject.AlwaysSearchOnLoad)
        {
            CurrentProject.SearchForGameAssets();
        }
        
        if (CurrentProject.SupportedFeatures.Contains(KnapperFeature.StringTableEditor))
        {
            projectLoadTasks.Add(($"StringTable ({CurrentProject.SelectedLocale})",
                SetLocaleAsync(CurrentProject.SelectedLocale)));
        }

        foreach (var asset in CurrentProject.GameAssets)
        {
            string assetType = asset.AssetType;
            if (assetType == GameConfig.Current.GlobalVariableExtension)
            {
                projectLoadTasks.Add((asset.GamePath, LoadGlobalVariablesAsync(asset.GamePath)));
            }
            else if (assetType == GameConfig.Current.QuestExtension)
            {
                projectLoadTasks.Add((asset.GamePath, LoadProjectQuestsAsync(asset.GamePath)));
            }
            else if (assetType == GameConfig.Current.ConversationManifestExtension)
            {
                projectLoadTasks.Add((asset.GamePath, LoadConvoBundleAsync(asset.GamePath)));
            }
        }
        
        var totalTasks = projectLoadTasks.Count;
        var completedTasks = 0;
        foreach (var task in projectLoadTasks)
        {
            await task.task;
            completedTasks++;
            progress.Report(new ProgressReport
            {
                Completed = completedTasks,
                Total = totalTasks,
                CurrentTask = $"Reading {task.path}"
            });
        }
    }
    
    private static async Task LoadGlobalVariablesAsync(string path)
    {
        var reader = new GlobalVariableReader(path);
        var variables = await reader.Read();
        await Task.Delay(100);
        GlobalVariables.AddRange(variables);
    }

    private static async Task LoadProjectQuestsAsync(string path)
    {
        return;
        var reader = new QuestBundleReader(path);
        var quests = await reader.Read();
        await Task.Delay(100);
        Quests.AddRange(quests);
    }
    
    private static async Task LoadConvoBundleAsync(string path)
    {
        var convoBundleReader = new ConvoBundleReader(path);
        var convos = await convoBundleReader.Read();
        await Task.Delay(100);
        
        Speakers.AddRange(convos.speakers);
        ConvoLookup.AddRange(convos.convos);
    }
    
    public static async Task<Conversation> LoadConversationAsync(string path)
    {
        var realPath = CurrentProject.GameAssets.FirstOrDefault(f => f.GamePath.Contains(path.Replace('/', '\\')));
        if (realPath == null)
        {
            throw new Exception($"Database: Could not find conversation {path}");
            return new Conversation();
        }
        var convoReader = new ConversationReader(realPath.GamePath);

        return await convoReader.Read();
    }
    
    public static async Task SetLocaleAsync(string locale)
    {
        var stringTablesInLocale = CurrentProject.GameAssets.Where(x => x.AssetType == GameConfig.Current.StringTableExtension && x.GamePath.Contains($@"\{locale}\text"));
        foreach (var table in stringTablesInLocale)
        {
            var reader = new StringTableReader(table.GamePath);
            var tables = await reader.Read();
            foreach (var stringTable in tables)
            {
                StringTable[stringTable.Name] = stringTable;
            }
        }
    }
}