using System.Text;
using OEIKnapper.Filesystem;

namespace OEIKnapper;

public class ObsidianArchiveFile
{
    public string Filepath;
    public List<OAFEntry> Items = [];

    public static ObsidianArchiveFile TryParse(string path)
    {
        var oaf = new ObsidianArchiveFile { Filepath = path };
        var fReader = new StreamReader(path);
        var bReader = new BinaryReader(fReader.BaseStream);

        char[] magic = bReader.ReadChars(4);
        if (new string(magic) != "OAF!")
        {
            throw new Exception("OAFReader: Got unexpected data. Ensure this is an Obsidian Archive File (.oaf) or check if file is corrupted.");
        }

        bReader.ReadBytes(8);
        long fileListPosition = bReader.ReadInt64();
        int fileCount = bReader.ReadInt32();
        Console.WriteLine($"Counted {fileCount} files");
        long position = bReader.BaseStream.Position;
            
        bReader.BaseStream.Seek(fileListPosition, SeekOrigin.Begin);
        while (bReader.BaseStream.CanRead && bReader.BaseStream.Position < bReader.BaseStream.Length)
        {
            string fname = OAFReader.ReadNullTerminatedString(bReader);
            fname = fname.Replace("\\", "/");
            oaf.Items.Add(new OAFEntry { Name = fname});
        }

        bReader.BaseStream.Seek(position, SeekOrigin.Begin);
        for (int i = 0; i < oaf.Items.Count; i++)
        {
            bReader.ReadInt32(); // magic, we ignore
            oaf.Items[i].DataOffset = bReader.ReadInt32();
            oaf.Items[i].Compressed = (bReader.ReadInt32() == 0x10) ? true : false;
            oaf.Items[i].UncompressedSize = bReader.ReadInt32();
            oaf.Items[i].CompressedSize = bReader.ReadInt32();
        }
        
        bReader.Close();
        fReader.Close();

        return oaf;
    }
}