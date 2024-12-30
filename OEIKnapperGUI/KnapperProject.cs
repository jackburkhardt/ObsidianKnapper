using System.IO;
using Newtonsoft.Json;

namespace OEIKnapperGUI;

public class KnapperProject
{
    public record GameAsset(string GamePath, bool IsBundled, string AssetType);
    
    public string Name { get; set; }
    public string Path { get; set; }
    public string GamePath { get; set; }
    public string UsesGameConfig { get; set; } = "DEFAULT";
    public string SelectedLocale { get; set; }
    public bool AlwaysSearchOnLoad { get; set; }
    public string[] AvailableLocales { get; set; } = [];
    public List<KnapperFeature> SupportedFeatures { get; set; }
    public List<GameAsset> GameAssets { get; set; } = new();
    
    public async void SearchForGameAssets()
    {
        GameRummager.RummageForGameFiles(this);
        GameRummager.RummageForLocales(this);
        
        var projectText = JsonConvert.SerializeObject(this, Formatting.Indented);
        await File.WriteAllTextAsync($@"{Path}\KnapperProject.cfg", projectText);
    }
    
}

public enum KnapperFeature
{
    ConvoEditor,
    StringTableEditor,
    OAFReader,
    GlobalVarEditor,
    QuestEditor,
}