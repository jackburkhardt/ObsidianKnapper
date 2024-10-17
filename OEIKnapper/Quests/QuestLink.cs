using OEIKnapper.Conversations;

namespace OEIKnapper.Quests;

public class QuestLink
{
    public int FromNodeID;
    public int ToNodeID;
    public List<Conditional> Conditionals;
    public List<string> ExtendedProperties;
}