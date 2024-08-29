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
            return false;
        }

        Type type = typeof(T);
        PropertyInfo[] properties = type.GetProperties();

        foreach (PropertyInfo property in properties)
        {
            object value1 = property.GetValue(obj);
            object value2 = property.GetValue(other);

            if (value1 == null && value2 == null)
            {
                continue;
            }
            else if (value1 == null || value2 == null)
            {
                return false;
            }
            else if (!value1.Equals(value2))
            {
                return false;
            }
        }

        return true;
    }

}
