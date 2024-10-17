﻿using Newtonsoft.Json.Linq;

namespace OEIKnapper.Conversations;

public abstract class Conditional
{
    public enum ComparisonType
    {
        AND,
        OR,
        EQUAL,
        NOT_EQUAL,
        GREATER_THAN,
        LESS_THAN,
        GREATER_THAN_OR_EQUAL,
        LESS_THAN_OR_EQUAL
    }
    
    public static ConditionalExpression TryParse(JToken json)
    {
        var cond = new ConditionalExpression
        {
            Conditions = [],
            Operator = (ComparisonType)(json["Operator"] ?? 0).Value<int>()
        };

        if (json["Components"] is JArray)
        {
            foreach (var item in json["Components"])
            {
                switch (item["$type"].Value<string>())
                {
                    case "OEIFormats.FlowCharts.ConditionalCall, OEIFormats":
                        cond.Conditions.Add(ConditionalCall.TryParse(item));
                        break;
                    case "OEIFormats.FlowCharts.ConditionalExpression, OEIFormats":
                        cond.Conditions.Add(ConditionalExpression.TryParse(item));
                        break;
                }
            }
        }
        
        return cond;
    }   
}