using System.Text;

namespace OEIKnapper.Filesystem;

public class OAFReader : FileReader
{
    public OAFReader(string path)
    {
        _path = path;
    }

    public async Task Read()
    {
        try {
            var fReader = new StreamReader(_path);
            var bReader = new BinaryReader(fReader.BaseStream);
            
            string magic = Encoding.UTF8.GetString(bReader.ReadBytes(4));
            if (magic != "OAF!")
            {
                throw new Exception("OAFReader: Unable to read magic string.");
            }
            
            var content = bReader.ReadBytes((int)bReader.BaseStream.Length);
            var contentString = Encoding.UTF8.GetString(content);
            
            RaiseFileParsedEvent(contentString, typeof(string), _path);
        }
        catch (Exception e)
        {
            RaiseFileParseFailedEvent(_path, $"{e.Source} -> {e.Message}");
        }
    }
}