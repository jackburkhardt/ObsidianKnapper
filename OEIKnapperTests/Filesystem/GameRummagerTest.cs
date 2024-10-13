using OEIKnapper.Filesystem;

namespace OEIKnapperTests.Filesystem;

[TestFixture]
[TestOf(typeof(GameRummager))]
public class GameRummagerTest
{
    
    [TestCase(@"C:\Program Files (x86)\Steam\steamapps\common\Pentiment")]
    public void RummageTest(string path)
    {
        var result = GameRummager.RummageForGameFiles(path);  
        result = result.ToList();
        
        Assert.That(result, Is.Not.Null);
        Assert.That(result, Is.Not.Empty);
        
        Console.WriteLine("Listing all found files:");
        foreach (var file in result)
        { 
            Console.WriteLine(file[path.Length..]);
        }
    }
}