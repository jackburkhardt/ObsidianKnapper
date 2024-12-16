using System.IO;
using OEIKnapperGUI;

namespace OEIKnapper.Filesystem;

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
        ConversationBundleExt,
        QuestExt,
        GlobalVariableExt,
        OAFExt
    };

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