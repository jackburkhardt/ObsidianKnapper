using OEIKnapper.Filesystem;

namespace OEIKnapper;

public class Database
{
    public record OperationProgress(int total = 0, int current = 0, string message = "");
    
    public static async Task LoadProject(string path)
    {
        var foundFiles = GameRummager.RummageForGameFiles(path);
        await GameRummager.AssignFilesToReaders(foundFiles);
    }
}