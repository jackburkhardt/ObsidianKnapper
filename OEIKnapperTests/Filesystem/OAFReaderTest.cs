using OEIKnapper.Filesystem;
using System.Threading;

namespace OEIKnapperTests.Filesystem;

public class OAFReaderTest
{
    [TestCase(@"C:\Program Files (x86)\Steam\steamapps\common\South Park - The Stick of Truth\data_archive_uncensored.oaf")]
    public void ReadTest(string path)
    {
       var reader = new OAFReader(path);
       // wait for the event to fire
       reader.OnFileParsedEvent += (data, type, s) =>
       {
           Console.WriteLine((string)data);
       };
       Task.Run(async () =>
       {
        await reader.Read();
       }).Wait();
       
    }
}