using Newtonsoft.Json.Linq;
using OEIKnapper;
using OEIKnapper.Filesystem;

namespace OEIKnapperTests.Filesystem;

[TestFixture]
[TestOf(typeof(StringTable))]
public class StringTableTest
{

    [TestCase(@"C:\Program Files (x86)\Steam\steamapps\common\Pentiment\Pentiment_Data\StreamingAssets\localized\enus\text\text_enus.stringtablebundle")]
    public void ReadTest(string path)
    {
        // Task.Run(async () =>
        // {
        //     var reader = new StringTableReader(path);
        //     var tables = await reader.Read();
        //     
        //     Assert.That(tables, Is.Not.Null);
        //     Assert.That(tables, Is.Not.Empty);
        //     
        //     foreach (var table in tables)
        //     {
        //         Console.WriteLine("Table: " + table.Name);
        //         foreach (var str in table.Strings)
        //         {
        //             Console.WriteLine("  " + str.ID + ": " + str.DefaultText);
        //         }
        //     }
        // }).Wait();
    }

    [TestCase(
        @"C:\Program Files (x86)\Steam\steamapps\common\Pentiment\Pentiment_Data\StreamingAssets\localized\enus\text\text_enus.stringtablebundle")]
    public void FileOccurrencesMatchesReadTest(string path)
    {
        // var readResult = new StringTableReader(path);
        // var tables = readResult.Read().Result;
        //
        // var fileText = File.ReadAllText(path);
        // var expectedTables = TestUtils.CountOccurrences(fileText, "\"Entries\"");
        //
        // Assert.That(tables, Has.Count.EqualTo(expectedTables));
        //
        // var expectedStrings = TestUtils.CountOccurrences(fileText, "\"ID\"");
        // var actualStrings = tables.SelectMany(table => table.Strings).Count();
        //
        // Assert.That(actualStrings, Is.EqualTo(expectedStrings));
    }
    
}