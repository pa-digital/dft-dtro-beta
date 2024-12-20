﻿using Newtonsoft.Json;

namespace DfT.DTRO.Extensions;

/// <summary>
/// Extension class
/// </summary>
public static class ObjectExtensions
{
    /// <summary>
    /// Beautify string to JSON
    /// </summary>
    /// <param name="obj">source object</param>
    /// <returns>JSON indented string</returns>
    public static string ToIndentedJsonString(this object obj) => JsonConvert.SerializeObject(obj, Formatting.Indented);

    /// <summary>
    /// Check if the string is of type enum
    /// </summary>
    /// <param name="value">Value to be checked</param>
    /// <param name="type">Name of the checked type</param>
    /// <returns><c>true</c> if is enum, otherwise <c>false</c></returns>
    public static bool IsEnum(this string value, string type)
    {
        List<string> items = type switch
        {
            "SourceActionType" => typeof(SourceActionType).GetDisplayNames<SourceActionType>().ToList(),
            "ProvisionActionType" => typeof(ProvisionActionType).GetDisplayNames<ProvisionActionType>().ToList(),
            "OrderReportingPointType" => typeof(OrderReportingPointType).GetDisplayNames<OrderReportingPointType>().ToList(),
            var _ => new List<string>()
        };

        return items.Contains(value);
    }

    /// <summary>
    /// Compare properties values
    /// </summary>
    /// <typeparam name="T">Type to check</typeparam>
    /// <param name="obj">First object to compare</param>
    /// <param name="other">Second object to compare</param>
    /// <returns><c>true</c> if similar values, otherwise <c>false</c></returns>
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

            if (value1 == null || value2 == null)
            {
                return false;
            }

            if (!value1.Equals(value2))
            {
                return false;
            }
        }

        return true;
    }

    /// <summary>
    /// Return string as integer
    /// </summary>
    /// <param name="source">Source to check</param>
    /// <returns><c>number</c> if parsing is successful, otherwise <c>zero</c></returns>
    public static int AsInt(this string source)
    {
        bool isDigit = int.TryParse(source, out int number);
        return isDigit ? number : 0;
    }

    /// <summary>
    /// Capture string between start and end strings
    /// </summary>
    /// <param name="source">Source string</param>
    /// <param name="before">Before item(s)</param>
    /// <param name="after">After item(s)</param>
    /// <returns>Captured string</returns>
    public static string GetBetween(this string source, string before, string after)
    {
        int start = source.IndexOf(before, StringComparison.Ordinal);
        int adjStart = start + before.Length;
        int end = source.LastIndexOf(after, StringComparison.Ordinal);
        return end <= adjStart ? "" : source.Substring(adjStart, end - adjStart);
    }

    public static DateTime ToDateTimeTruncated(this DateTime dateTime)
    {
        return new DateTime(
            dateTime.Year,
            dateTime.Month,
            dateTime.Day,
            dateTime.Hour,
            dateTime.Minute,
            dateTime.Second,
            DateTimeKind.Utc
        );
    }

    public static string ToBackwardCompatibility(this string source, SchemaVersion schemaVersion)
    {
        if (schemaVersion >= new SchemaVersion("3.3.0"))
        {
            return source;
        }

        var parts = source.Split('.');
        if (parts.Length > 1)
        {
            var first = parts[0];
            var last = parts[1];
            var chars = last.ToCharArray();
            var toLower = chars[0].ToString().ToLower();
            var item = $"{first}.{toLower + string.Join("", chars.Skip(1))}";
            return item;
        }
        else
        {
            var first = parts[0];
            var chars = first.ToCharArray();
            var toLower = chars[0].ToString().ToLower();
            var item = $"{toLower + string.Join("", chars.Skip(1))}";
            return item;
        }
    }

    public static bool IsPascalCase(this string source)
    {
        if (string.IsNullOrEmpty(source))
        {
            return false;
        }

        var first = source.First();
        return char.IsUpper(first);
    }
}
