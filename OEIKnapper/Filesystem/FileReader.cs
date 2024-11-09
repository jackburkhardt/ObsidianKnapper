namespace OEIKnapper.Filesystem;

public class FileReader
{
    protected string _path;
    
    public delegate void OnFileParseComplete(object data, Type type, string path);
    public event OnFileParseComplete OnFileParsedEvent;
    protected void RaiseFileParsedEvent(object data, Type type, string path) => OnFileParsedEvent?.Invoke(data, type, path);
   
    public delegate void OnFileParseFailed(string path, string message);
    public event OnFileParseFailed OnFileParseFailedEvent;
    protected void RaiseFileParseFailedEvent(string path, string message) => OnFileParseFailedEvent?.Invoke(path, message);

}