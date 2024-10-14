namespace OEIKnapper;

public class PlayerResponseNode : Node
{
    public int Persistence;
    
    public static PlayerResponseNode TryParse(Node baseNode)
    {
        PlayerResponseNode newNode = new PlayerResponseNode
        {
            NodeID = baseNode.NodeID,
            Links = baseNode.Links,
            Conditionals = baseNode.Conditionals,
        };

        return newNode;
    }
}