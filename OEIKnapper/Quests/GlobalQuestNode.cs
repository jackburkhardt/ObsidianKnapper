using Newtonsoft.Json.Linq;
using OEIKnapper.Conversations;

namespace OEIKnapper.Quests;

public class GlobalQuestNode : Node
{
    public static GlobalQuestNode TryParse(JToken json)
    {
        return new GlobalQuestNode();
    }
}