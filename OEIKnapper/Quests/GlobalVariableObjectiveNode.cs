using Newtonsoft.Json.Linq;

namespace OEIKnapper.Quests;

public class GlobalVariableObjectiveNode : QuestEventGlobalVariableNode
{
    public int DescriptionID;
    public int ParentBranchID;

    public static GlobalVariableObjectiveNode TryParse(JToken json)
    {
        return new GlobalVariableObjectiveNode
        {
            DescriptionID = json["DescriptionID"].Value<int>(),
            ParentBranchID = json["ParentBranchID"].Value<int>(),
        };
    }
}