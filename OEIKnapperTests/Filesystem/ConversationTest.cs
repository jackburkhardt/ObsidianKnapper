using Newtonsoft.Json;
using OEIKnapper.Filesystem;

namespace OEIKnapperTests.Filesystem;

[TestFixture]
public class ConversationTest
{
    [TestCase(@"C:\Program Files (x86)\Steam\steamapps\common\Pentiment\Pentiment_Data\StreamingAssets\design\conversations\act3\act3_simon.conversationasset")]
    public void ReadTest(string path)
    {
        // Task.Run(async () =>
        // {
        //     var reader = new ConversationReader(path);
        //     var convo = await reader.Read();
        //     
        //     Assert.That(convo, Is.Not.Null);
        //
        //     var json = JsonConvert.SerializeObject(convo, Formatting.Indented,
        //         new JsonSerializerSettings { TypeNameHandling = TypeNameHandling.Objects });
        //     
        //     Console.WriteLine(json);
        // }).Wait();
    }
}