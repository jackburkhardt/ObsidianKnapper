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
            SpeakerGuid = Guid.Parse(json["SpeakerGuid"].Value<string>()),
            ListenerGuid = Guid.Parse(json["ListenerGuid"].Value<string>()),
            HasVO = json["HasVO"].Value<bool>()
        };
    }
}