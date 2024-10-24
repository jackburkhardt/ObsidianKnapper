using Newtonsoft.Json.Linq;
using OEIKnapper;
using OEIKnapper.Filesystem;

namespace OEIKnapperTests.Filesystem;

[TestFixture]
[TestOf(typeof(ConvoBundleReader))]
public class ConvoBundleTest
{

    [TestCase(@"C:\Program Files (x86)\Steam\steamapps\common\Pentiment\Pentiment_Data\StreamingAssets\design\conversations\conversations.conversationbundle")]
    public void ReadTest(string path)
    {
        Task.Run(async () =>
        {
            var reader = new ConvoBundleReader(path);
            var tables = await reader.Read();
            
            Assert.That(tables.Item1, Is.Not.Empty);
            Assert.That(tables.Item2, Is.Not.Empty);
            
            foreach (var speaker in tables.Item1)
            {
                Console.WriteLine("Speaker: " + speaker.Tag);
                Console.WriteLine("  ID: " + speaker.ID);
            }

            foreach (var convo in tables.Item2)
            {
                Console.WriteLine("Conversation: " + convo.Name);
                Console.WriteLine("  ID: " + convo.ID);
                Console.WriteLine("  Filename: " + convo.Tag);
            }
        }).Wait();
    }

    [TestCase(
        @"C:\Program Files (x86)\Steam\steamapps\common\Pentiment\Pentiment_Data\StreamingAssets\design\conversations\conversations.conversationbundle")]
    public void FileOccurrencesMatchesReadTest(string path)
    {
        var readResult = new StringTableReader(path);
        var tables = readResult.Read().Result;
        
        var fileText = File.ReadAllText(path);
        var expectedTables = TestUtils.CountOccurrences(fileText, "\"Entries\"");
        
        Assert.That(tables, Has.Count.EqualTo(expectedTables));
        
        var expectedStrings = TestUtils.CountOccurrences(fileText, "\"ID\"");
        var actualStrings = tables.SelectMany(table => table.Strings).Count();
        
        Assert.That(actualStrings, Is.EqualTo(expectedStrings));
    }
    
}