using System;
using System.Collections.Generic;
using System.Linq;
using DfT.DTRO.Enums;
using Newtonsoft.Json;

namespace DfT.DTRO.Extensions;
public static class ObjectExtensions
{
    public static string ToIndentedJsonString(this object obj)
    {
        return JsonConvert.SerializeObject(obj, Formatting.Indented);
    }

    public static bool IsEnum(this string value, string type)
    {
        List<string> items = type switch
        {
            "SourceActionType" => typeof(SourceActionType).GetDisplayName<SourceActionType>().ToList(),
            "ProvisionActionType" => typeof(ProvisionActionType).GetDisplayName<ProvisionActionType>().ToList(),
            _ => new List<string>()
        };

        return items.Contains(value);
    }
}