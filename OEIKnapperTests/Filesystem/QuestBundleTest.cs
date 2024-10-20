using Newtonsoft.Json;
using OEIKnapper.Filesystem;

namespace OEIKnapperTests.Filesystem;

[TestFixture]
public class QuestBundleTest
{
    [TestCase(@"C:\Program Files (x86)\Steam\steamapps\common\Pentiment\Pentiment_Data\StreamingAssets\design\quests\quests.questbundle")]
    public void ReadTest(string path)
    {
        Task.Run(async () =>
        {
            var reader = new QuestBundleReader(path);
            var quests = await reader.Read();
            
            Assert.That(quests, Is.Not.Empty);

            foreach (var quest in quests)
            {
                Console.WriteLine("Quest: " + quest.Filename);
                Console.WriteLine("  ID: " + quest.ID);
                Console.WriteLine("  ExperienceWeight: " + quest.TotalExperienceWeight);
                Console.WriteLine("  Nodes:");
                var nodes = JsonConvert.SerializeObject(quest.Nodes, Formatting.Indented,
                    new JsonSerializerSettings { TypeNameHandling = TypeNameHandling.Objects });
                Console.WriteLine(nodes);
            }
            
        }).Wait();
    }
}