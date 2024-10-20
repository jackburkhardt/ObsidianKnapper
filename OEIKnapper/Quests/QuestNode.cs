using Newtonsoft.Json.Linq;
using OEIKnapper.Conversations;

namespace OEIKnapper.Quests;

public class QuestNode : Node
{
    public static QuestNode TryParse(JToken json)
    {
        return new QuestNode();
    }
}