using Newtonsoft.Json.Linq;

namespace OEIKnapper.Quests;

public abstract class QuestNode
{
    public int NodeID;

    public static QuestNode TryParse(JToken json)
    {
        
    }
}