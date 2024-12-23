using Newtonsoft.Json;
using Newtonsoft.Json.Linq;

namespace OEIKnapper.Conversations;

/// <summary>
/// Defines a function with parameters that is evaluated as part of a <see cref="ConditionalExpression"/>.
/// </summary>
public class ConditionalCall : Conditional
{
    public string Function;
    public List<string> Parameters = [];
    public bool Not;
    
    public static ConditionalCall TryParse(JToken json)
    {
        if (!OEIJsonUtils.ValidateObject(json, "Data"))
        {
            throw new ArgumentException("Unable to parse ConditionalCall from: " + json.ToString(Formatting.None));
        }
        var funcName = json["Data"]["FullName"].Value<string>();
        var parameters = json["Data"]["Parameters"].ToObject<List<string>>();
        
        return new ConditionalCall
        {
            Function = funcName,
            Parameters = parameters,
            Not = json["Not"]?.Value<bool>() ?? false
        };
    }
    
    public override string ToString()
    {
        int start = Function.IndexOf('(');
        int end = Function.IndexOf(')');
        string func = Function.Substring(start+1, end - start - 1);
        string funcWithParams = Parameters.Count > 0 ? Function.Replace(func, string.Join(", ", Parameters)) : Function;
        return $"{(Not ? "!" : "")}{funcWithParams}";
    }
}