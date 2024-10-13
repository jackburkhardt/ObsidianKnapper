namespace OEIKnapper;

public abstract class Node
{
    public int NodeID;
    public List<DialogueLink> Links;
    //todo: ExtendedProperties
    public List<ConditionalCall> Conditionals;
}