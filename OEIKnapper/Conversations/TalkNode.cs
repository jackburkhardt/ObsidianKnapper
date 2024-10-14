using Newtonsoft.Json.Linq;

namespace OEIKnapper;

public class TalkNode : Node
{
    public Guid SpeakerGuid;
    public Guid ListenerGuid;
    
    public static TalkNode TryParse(JToken json)
    {
        return new TalkNode();
    }
}