using OEIKnapper.Filesystem;

namespace OEIKnapperTests.Filesystem;

[TestFixture]
[TestOf(typeof(GlobalVariableReader))]
public class GlobalVariableReaderTest
{

    [TestCase(@"C:\Program Files (x86)\Steam\steamapps\common\Pentiment\Pentiment_Data\StreamingAssets\design\globalvariables\game.globalvariablesbundle")]
    public void ReadTest(string path)
    {
        // Task.Run(async () =>
        // {
        //     var reader = new GlobalVariableReader(path);
        //     var variables = await reader.Read();
        //     
        //     Assert.That(variables, Is.Not.Empty);
        //     
        //     foreach (var variable in variables)
        //     {
        //         Console.WriteLine("Tag: " + variable.Tag);
        //         Console.WriteLine("  ID: " + variable.ID);
        //         Console.WriteLine("  Type: " + variable.Type);
        //         Console.WriteLine("  InitialValue: " + variable.InitialValue);
        //     }
        // }).Wait();
    }
    
    
}