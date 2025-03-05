using System.Collections;
using System.Text.RegularExpressions;

enum Casing
{
    Camel,
    Pascal
}

public class CasingValidationService
{
    HashSet<string> keysToConvert = Constants.KeysToConvert;

    private bool IsObjectOrArray(object value)
    {
        return value is IDictionary<string, object> || value is IEnumerable<object>;
    }
    public ExpandoObject ConvertKeysToPascalCase(ExpandoObject obj)
    {
        var result = new ExpandoObject();
        var dictionary = (IDictionary<string, object>)obj;
        var resultDict = (IDictionary<string, object>)result;

        foreach (var kvp in dictionary)
        {
            string newKey = kvp.Key != "_comment" && keysToConvert.Contains(kvp.Key.ToLower()) && IsObjectOrArray(kvp.Value) ? ToPascalCase(kvp.Key) : kvp.Key;
            resultDict[newKey] = ConvertValue(kvp.Value);
        }

        return result;
    }

    private object ConvertValue(object value)
    {
        if (value is ExpandoObject expando)
        {
            return ConvertKeysToPascalCase(expando);
        }
        else if (value is IList list)
        {
            var newList = new List<object>();
            foreach (var item in list)
            {
                if (item is ExpandoObject expandoItem)
                {
                    newList.Add(ConvertKeysToPascalCase(expandoItem));
                }
                else
                {
                    newList.Add(item);
                }
            }
            return newList;
        }
        return value;
    }

    private string ToPascalCase(string str)
    {
        return Regex.Replace(str, @"(^|_|\-|\s)(\w)", m => m.Groups[2].Value.ToUpper());
    }

    public List<string> ValidateCamelCase(ExpandoObject data)
    {
        List<string> invalidProperties = new();
        CheckCase(data, invalidProperties, Casing.Camel);

        return invalidProperties;
    }

    public List<string> ValidatePascalCase(ExpandoObject data)
    {
        List<string> invalidProperties = new();
        CheckCase(data, invalidProperties, Casing.Pascal);

        return invalidProperties;
    }

    public bool SchemaVersionEnforcesCamelCase(SchemaVersion schemaVersion)
    {
        return schemaVersion.Major > 3 ||
            (schemaVersion.Major == 3 && schemaVersion.Minor > 3) ||
            (schemaVersion.Major == 3 && schemaVersion.Minor == 3 && schemaVersion.Patch >= 2);
    }

    private void CheckCase(object obj, List<string> invalidProperties, Casing casing)
    {
        Func<string, bool> func = casing == Casing.Camel ? IsCamelCase : IsPascalCase;
        if (obj is ExpandoObject expandoObj)
        {
            var dict = (IDictionary<string, object>)expandoObj;

            foreach (var kvp in dict)
            {
                string key = kvp.Key;
                if (key == "_comment")
                {
                    continue;
                }

                if (casing == Casing.Pascal && keysToConvert.Contains(key.ToLower()) &&  IsObjectOrArray(kvp.Value) && !func(key))
                {
                    invalidProperties.Add(key);
                }

                if (casing == Casing.Camel && !func(key))
                {

                    invalidProperties.Add(key);
                }

                // Recursively check the value, if it's an ExpandoObject or a list
                if (kvp.Value is ExpandoObject || kvp.Value is List<object>)
                {
                    CheckCase(kvp.Value, invalidProperties, casing);
                }
            }
        }
        else if (obj is List<object> listObj)
        {
            foreach (var item in listObj)
            {
                CheckCase(item, invalidProperties, casing);
            }
        }
    }

    private bool IsCamelCase(string input)
    {
        return !string.IsNullOrEmpty(input) && char.IsLower(input[0]);
    }

    private bool IsPascalCase(string input)
    {
        return !string.IsNullOrEmpty(input) && char.IsUpper(input[0]);
    }
}