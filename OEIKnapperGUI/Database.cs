using System.IO;
using Newtonsoft.Json;
using OEIKnapper;
using OEIKnapper.Conversations;
using OEIKnapper.Filesystem;
using OEIKnapper.Quests;
using OEIKnapperGUI;

namespace OEIKnapper;

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

    public static async Task<KnapperProject> FindOrCreateProjectAsync(string gamePath)
    {
        if (!Directory.Exists(Environment.GetFolderPath(Environment.SpecialFolder.MyDocuments) + @"\KnapperProjects"))
        {
            Directory.CreateDirectory(Environment.GetFolderPath(Environment.SpecialFolder.MyDocuments) + @"\KnapperProjects");
        }
        
        KnapperProject project = new KnapperProject 
        {
            Name = Path.GetFileNameWithoutExtension(gamePath),
            GamePath = gamePath,
            SelectedLocale = "enus"
        };
        project.Path = Environment.GetFolderPath(Environment.SpecialFolder.MyDocuments) + $@"\KnapperProjects\{project.Name}";
        if (!Directory.Exists(project.Path))
        {
            Directory.CreateDirectory(project.Path);
        }
        
        if (File.Exists($@"{project.Path}\project.config"))
        {
            var fileText = await File.ReadAllTextAsync($@"{project.Path}\project.config");
            var parsedProject = JsonConvert.DeserializeObject<KnapperProject>(fileText);
            if (parsedProject != null)
            {
                project = parsedProject;
            }
        }

        
        GameRummager.RummageForGameFiles(ref project);
        
        var projectText = JsonConvert.SerializeObject(project, Formatting.Indented);
        await File.WriteAllTextAsync($@"{project.Path}\project.config", projectText);
        
        return project;
    }
    
    public static async Task LoadProjectAsync(string gamePath, IProgress<ProgressReport> progress)
    {
        var projectLoadTasks = new List<(string path, Task task)>();
        
        CurrentProject = await FindOrCreateProjectAsync(gamePath);

        projectLoadTasks.Add(($"StringTable ({CurrentProject.SelectedLocale})",SetLocaleAsync(CurrentProject.SelectedLocale)));
        
        foreach (var asset in CurrentProject.GameAssets)
        {
            switch (asset.AssetType)
            {
                case GameRummager.GlobalVariableExt:
                    projectLoadTasks.Add((asset.GamePath, LoadGlobalVariablesAsync(asset.GamePath)));
                    break;
                case GameRummager.QuestExt:
                    projectLoadTasks.Add((asset.GamePath, LoadProjectQuestsAsync(asset.GamePath)));
                    break;
                case GameRummager.ConversationBundleExt:
                    projectLoadTasks.Add((asset.GamePath, LoadConvoBundleAsync(asset.GamePath)));
                    break;
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
        var convoReader = new ConversationReader(path);

        return await convoReader.Read();
    }
    
    public static async Task SetLocaleAsync(string locale)
    {
        var foundFile = CurrentProject.GameAssets.FirstOrDefault(f => f.GamePath.Contains($"text_{locale}.{GameRummager.StringTableExt}"));
        if (foundFile == null)
        {
            throw new Exception($"Database: Could not find string table for locale {locale}");
            return;
        }
        
        var stringTableReader = new StringTableReader(foundFile.GamePath);
        var tables = await stringTableReader.Read();
        
        StringTable.Clear();
        foreach (var table in tables)
        {
            StringTable.Add(table.Name, table);
        }
        CurrentProject.SelectedLocale = locale;
    }
}