using Newtonsoft.Json.Linq;

namespace OEIKnapper.Conversations;

public class BankNode : Node
{
    public List<int> ChildNodeIDs;
    
    public static BankNode TryParse(JToken json)
    {
        if (!OEIJsonUtils.ValidateObject(json, "ChildNodeIDs"))
        {
            throw new ArgumentException("BankNode is missing ChildNodeIDs");
        }
        
        return new BankNode
        {
            ChildNodeIDs = json["ChildNodeIDs"].Values<int>().ToList()
        };
    }
}