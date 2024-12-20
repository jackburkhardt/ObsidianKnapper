using Newtonsoft.Json;
using Newtonsoft.Json.Linq;

namespace OEIKnapper.Conversations;

/// <summary>
/// This is data that is used within the OnEnter, OnExit, and OnUpdate events of a <see cref="Node"/>. Not to be confused with <see cref="ScriptNode"/>. 
/// </summary>
public class NodeScriptItem
{
    public ConditionalCall Functions { get; set; }
    public ConditionalExpression Conditional { get; set; }
    
    public static NodeScriptItem TryParse(JToken json)
    {
        if (!OEIJsonUtils.ValidateObject(json, "Data", "Conditional"))
        {
            throw new ArgumentException("Unable to parse NodeScript from: " + json.ToString(Formatting.None));
        }
        return new NodeScriptItem
        {
            Functions = ConditionalCall.TryParse(json),
            Conditional = OEIKnapper.Conversations.Conditional.TryParse(json["Conditional"])
        };
    }
}