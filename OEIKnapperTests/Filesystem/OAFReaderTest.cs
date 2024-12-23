using OEIKnapper.Filesystem;
using System.Threading;

namespace OEIKnapperTests.Filesystem;

public class OAFReaderTest
{
    [TestCase(@"C:\Program Files (x86)\Steam\steamapps\common\South Park - The Stick of Truth\data_localized_en.oaf")]
    public void ReadTest(string path)
    {
        var reader = new OAFReader(path);
        var oaf = reader.Read().Result;
        
        Assert.That(oaf, Is.Not.Null);
        Assert.That(oaf.Items, Is.Not.Empty);
        
        Console.WriteLine("Listing all found files:");
        foreach (var file in oaf.Items)
        { 
            Console.WriteLine(file.Name);
        }
    }
}