using System;
using System.Collections.Generic;
using System.Linq;
using System.Reflection;
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

    public static bool ComparePropertiesValues<T>(this T obj, T other)
    {
        if (obj == null || other == null)
        {
            return false; // If either object is null, they are not equal
        }

        Type type = typeof(T);
        PropertyInfo[] properties = type.GetProperties();

        foreach (PropertyInfo property in properties)
        {
            object value1 = property.GetValue(obj);
            object value2 = property.GetValue(other);

            if (value1 == null && value2 == null)
            {
                continue; // Both values are null, consider them equal
            }
            else if (value1 == null || value2 == null)
            {
                return false; // One value is null, the other is not, consider them not equal
            }
            else if (!value1.Equals(value2))
            {
                return false; // Values are not equal, objects are not equal
            }
        }

        return true; // All properties are equal
    }
}
