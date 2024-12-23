using System.Text;

namespace OEIKnapper.Filesystem;

// Much of the OAF format knowledge is adapted from ohegba: https://github.com/ohegba/ObsidianArchiveExtractor
public class OAFReader : FileReader
{
    public OAFReader(string path)
    {
        _path = path;
    }
    
    public static string ReadNullTerminatedString(BinaryReader reader)
    {
        var sb = new StringBuilder();
        char c;
        while (reader.BaseStream.Position < reader.BaseStream.Length && (c = reader.ReadChar()) != '\0')
        {
            sb.Append(c);
        }
        return sb.ToString();
    }

    public async Task<ObsidianArchiveFile> Read()
    {
            var oaf = ObsidianArchiveFile.TryParse(_path);
            return oaf;
    }
}