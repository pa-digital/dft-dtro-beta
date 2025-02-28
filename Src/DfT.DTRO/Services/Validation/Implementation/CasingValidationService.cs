using System.Collections;
using System.Text.RegularExpressions;

public class CasingValidationService
{
    HashSet<string> keysToConvert = new HashSet<string> { "source", "provision", "regulatedplace", "geometry", "lineargeometry", "pointgeometry", "polygon", "directedlinear", "externalreference", "uniquestreetreferencenumber", "elementarystreetunit", "regulation", "speedlimitvaluebased", "speedlimitprofilebased", "generalregulation", "offlistregulation", "condition", "ratetable", "temporaryregulation", "roadcondition", "numberofoccupants", "occupantcondition", "drivercondition", "accesscondition", "timevalidity", "nonvehicularroaduser", "permitcondition", "vehiclecharacteristics", "conditionset", "authority", "permitsubjecttofee", "period", "timeperiodofday", "specialday", "publicholiday", "dayweekmonth", "changeabletimeperiod", "changeabletimeperiodstart", "changeabletimeperiodend", "calendarweekinmonth", "weekofmonth", "instanceofdaywithinmonth", "changeabletimeperiodsource", "changeabletimeperiodentry", "maximumgrossweightcharacteristic", "maximumheightcharacteristic", "maximumlengthcharacteristic", "maximumwidthcharacteristic", "heaviestaxleweightcharacteristic", "numberofaxlescharacteristic", "emissions", "ratetable,", "ratelinecollection", "rateline" };

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
        CheckCamelCase(data, invalidProperties);

        return invalidProperties;
    }

    public bool SchemaVersionEnforcesCamelCase(SchemaVersion schemaVersion)
    {
        return schemaVersion.Major > 3 ||
            (schemaVersion.Major == 3 && schemaVersion.Minor > 3) ||
            (schemaVersion.Major == 3 && schemaVersion.Minor == 3 && schemaVersion.Patch >= 2);
    }

    private void CheckCamelCase(object obj, List<string> invalidProperties)
    {
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

                if (!IsCamelCase(key))
                {
                    invalidProperties.Add(key);
                }

                // Recursively check the value, if it's an ExpandoObject or a list
                if (kvp.Value is ExpandoObject || kvp.Value is List<object>)
                {
                    CheckCamelCase(kvp.Value, invalidProperties);
                }
            }
        }
        else if (obj is List<object> listObj)
        {
            foreach (var item in listObj)
            {
                CheckCamelCase(item, invalidProperties);
            }
        }
    }

    private bool IsCamelCase(string key)
    {
        return Regex.IsMatch(key, "^[a-z][a-zA-Z0-9]*$");
    }
}