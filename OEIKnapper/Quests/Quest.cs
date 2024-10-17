namespace OEIKnapper.Quests;

public class Quest
{
    public Guid ID;
    public string Filename;
    public int TotalExperienceWeight;
    public List<QuestNode> Nodes;
}