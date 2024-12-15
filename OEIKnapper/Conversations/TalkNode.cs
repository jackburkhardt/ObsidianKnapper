using Newtonsoft.Json.Linq;

namespace OEIKnapper.Conversations;

public class TalkNode : Node
{
    public Guid SpeakerGuid;
    public Guid ListenerGuid;
    public bool HasVO;
    
    public static TalkNode TryParse(JToken json)
    {
        return new TalkNode
        {
            SpeakerGuid = Guid.Parse(json["SpeakerGuid"]?.Value<string>() ?? Guid.Empty.ToString()),
            ListenerGuid = Guid.Parse(json["ListenerGuid"]?.Value<string>() ?? Guid.Empty.ToString()),
            HasVO = json["HasVO"]?.Value<bool>() ?? false
        };
    }
}