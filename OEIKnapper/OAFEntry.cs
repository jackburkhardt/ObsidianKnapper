namespace OEIKnapper;

public class OAFEntry
{
    public string Name { get; init; }
    public int DataOffset { get; set; }
    public bool Compressed { get; set; }
    public int UncompressedSize { get; set; }
    public int CompressedSize { get; set; }
    public byte[] Payload;
    
    public void ReadEntry(string containingArchivePath)
    {
        using var fs = new FileStream(containingArchivePath, FileMode.Open, FileAccess.Read);
        using var reader = new BinaryReader(fs);
        reader.BaseStream.Seek(DataOffset, SeekOrigin.Begin);
        
        int size = Compressed ? CompressedSize : UncompressedSize;
        byte[] payload = reader.ReadBytes(size);
        
        fs.Close();
        reader.Close();
        Payload = payload;
    }
}