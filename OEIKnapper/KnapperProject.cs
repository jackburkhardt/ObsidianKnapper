namespace OEIKnapper;

public class KnapperProject
{
    public record GameAsset(string GamePath, bool IsBundled, string AssetType);
    
    public string Name { get; set; }
    public string Path { get; set; }
    public string GamePath { get; set; }
    public string SelectedLocale { get; set; } = "enus";
    public List<KnapperFeature> SupportedFeatures { get; set; }
    public List<GameAsset> GameAssets { get; set; } = new();
    
}

public enum KnapperFeature
{
    ConvoEditor,
    StringTableEditor,
    OAFReader,
    GlobalVarEditor,
    QuestEditor,
}