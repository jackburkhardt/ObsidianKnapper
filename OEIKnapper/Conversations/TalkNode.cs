namespace OEIKnapper;

public class TalkNode : Node
{
    public Guid SpeakerGuid;
    public Guid ListenerGuid;
    
    public static TalkNode TryParse(Node baseNode)
    {
        TalkNode newNode = new TalkNode
        {
            NodeID = baseNode.NodeID,
            Links = baseNode.Links,
            Conditionals = baseNode.Conditionals,
        };

        return newNode;
    }
}