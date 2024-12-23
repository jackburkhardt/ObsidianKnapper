using System.Text;

namespace OEIKnapper.Filesystem;

// Much of the OAF format knowledge is adapted from ohegba: https://github.com/ohegba/ObsidianArchiveExtractor
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

            char[] magic = bReader.ReadChars(4);
            if (!magic.Equals("OAF!"))
            {
                throw new Exception("OAFReader: The provided file is not an Obsidian Archive File (.OAF) or is corrupted.");
            }

            bReader.ReadBytes(8);
            
            Int64 fileListPosition = bReader.ReadInt64();
            Int32 fileCount = bReader.ReadInt32();
            Int64 position = bReader.BaseStream.Position;
            
            bReader.BaseStream.Seek(fileListPosition, SeekOrigin.Begin);
            
            while (bReader.BaseStream.CanRead && bReader.BaseStream.Position < bReader.BaseStream.Length)
            {
                Int64 fileOffset = bReader.ReadInt64();
                Int64 fileSize = bReader.ReadInt64();
                Int32 fileNameLength = bReader.ReadInt32();
                string fileName = Encoding.UTF8.GetString(bReader.ReadBytes(fileNameLength));
                
                bReader.BaseStream.Seek(fileOffset, SeekOrigin.Begin);
                byte[] fileData = bReader.ReadBytes((int)fileSize);
                
                // Do something with the fileData
                
                bReader.BaseStream.Seek(position, SeekOrigin.Begin);
            }
            
        }
        catch (Exception e)
        {

        }
    }
}