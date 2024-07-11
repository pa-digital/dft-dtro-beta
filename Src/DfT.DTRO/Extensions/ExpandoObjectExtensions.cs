using System;
using System.Collections.Generic;
using System.Dynamic;
using System.Linq;

namespace DfT.DTRO.Extensions;

/// <summary>
/// Provides extension methods to retrieve data by path from <see cref="ExpandoObject"/> instances.
/// </summary>
public static class ExpandoObjectExtensions
{
    /// <summary>
    /// Extension to retrieve an object value from a collection of Expando Object key paths else the default for the data type.
    /// </summary>
    /// <param name="source">Object to be queried.</param>
    /// <param name="paths">The path to the field to be retrieved.</param>
    /// <typeparam name="T">The field type.</typeparam>
    /// <returns>An object value from a collection of Expando Object key paths else the default for the data type.</returns>
    public static T GetValueOrDefault<T>(this ExpandoObject source, IEnumerable<string> paths)
    {
        if (source is null)
        {
            return default;
        }

        var nextKey = paths.FirstOrDefault();

        if (nextKey is null)
        {
            return source is T result ? result : default;
        }

        var bracketIndex = nextKey.IndexOf('[');

        if (bracketIndex != -1)
        {
            // TODO: Implement paths with indexes once needed.
            return default;
        }

        var remaining = paths.Skip(1);

        if (!remaining.Any())
        {
            return source.GetFieldValueOrDefault<T>(nextKey);
        }

        var next = source.GetExpandoOrDefault(nextKey);

        return next.GetValueOrDefault<T>(remaining);
    }

    /// <summary>
    /// Extension to retrieve an object path value from an Expando Object from a list collection, else the default for the data type.
    /// </summary>
    /// <param name="source">Object to be queried.</param>
    /// <param name="paths">The path to the field to be retrieved.</param>
    /// <typeparam name="T">The field type.</typeparam>
    /// <returns>An object path value from an Expando Object from a list collection, else the default for the data type.</returns>
    public static T GetValueOrDefault<T>(this IList<T> source, IEnumerable<string> paths)
    {
        if (!paths.Any())
        {
        }

        throw new NotImplementedException();
    }

    /// <summary>
    /// Extension to retrieve an object value from a single Expando Object key path else the default for the data type.
    /// </summary>
    /// <param name="source">Object to be queried.</param>
    /// <param name="path">The path to the field to be retrieved.</param>
    /// <typeparam name="T">The field type.</typeparam>
    /// <returns>An object value from a single Expando Object key path else the default for the data type.</returns>
    public static T GetValueOrDefault<T>(this ExpandoObject source, string path)
        => (path.Split('.') is string[] split && split.Length > 1)
            ? source.GetValueOrDefault<T>(split)
            : source.GetFieldValueOrDefault<T>(path);

    public static void PutValue<T>(this ExpandoObject source, string path, T value)
    {
        if (path.Split('.') is string[] split && split.Length > 1)
        {
            source.PutValue(split, value);
        }
        else
        {
            source.SetFieldValue(path, value);
        }
    }

    private static void SetFieldValue<T>(this ExpandoObject source, string field, T value)
    {
        var dict = (IDictionary<string, object>)source;
        dict[field] = value;
    }

    private static void PutValue<T>(this ExpandoObject source, string[] path, T value)
    {
        if (path.Length == 1)
        {
            source.SetFieldValue(path[0], value);
            return;
        }

        var current = source as IDictionary<string, object>;

        for (int i = 0; i < path.Length - 1; i++)
        {
            if (!current.ContainsKey(path[i]))
            {
                current[path[i]] = new ExpandoObject();
            }

            current = current[path[i]] as IDictionary<string, object>;
        }

        current[path.Last()] = value;
    }

    /// <summary>
    /// Extension to retrieve a field from an Expando object.
    /// </summary>
    /// <param name="source">Object to be queried.</param>
    /// <param name="key">The key for a field to be retrieved.</param>
    /// <returns>A field from an Expando object.</returns>
    public static object GetField(this ExpandoObject source, string key)
    {
        var dict = source as IDictionary<string, object>;

        return dict[key];
    }

    /// <summary>
    /// Extension to check if Expando object has field with given name.
    /// </summary>
    /// <param name="source">Object to be queried.</param>
    /// <param name="key">The key for a field to be checked for existence.</param>
    /// <returns>A field from an Expando object.</returns>
    public static bool HasField(this ExpandoObject source, string key)
    {
        var dict = source as IDictionary<string, object>;

        return dict.ContainsKey(key);
    }

    /// <summary>
    /// Extension to retrieve an inner Expando object from another.
    /// </summary>
    /// <param name="source">Object to be queried.</param>
    /// <param name="key">The key for the inner object.</param>
    /// <returns>An inner Expando object contained in another.</returns>
    public static ExpandoObject GetExpando(this ExpandoObject source, string key)
    {
        if (source.GetField(key) is not ExpandoObject expando)
        {
            throw new InvalidOperationException($"The field under '{key}' was not an expando type.");
        }

        return expando;
    }

    /// <summary>
    /// Extension to retrieve an inner Expando object value.
    /// </summary>
    /// <param name="source">Object to be queried.</param>
    /// <param name="key">The key for the inner object.</param>
    /// <returns>An inner Expando object value.</returns>
    /// <typeparam name="T">Type of returned value.</typeparam>
    public static T GetValue<T>(this ExpandoObject source, string key)
    {
        if (source.GetField(key) is not T result)
        {
            throw new InvalidOperationException($"The field under '{key}' was not {typeof(T).FullName}");
        }

        return result;
    }

    /// <summary>
    /// Extension to retrieve DateTime or null from ExpandoObject.
    /// </summary>
    /// <param name="source">Object to be queried.</param>
    /// <param name="key">The key for the DateTime object.</param>
    /// <returns>A DateTime object.</returns>
    public static DateTime? GetDateTimeOrNull(this ExpandoObject source, string key)
    {
        if (!(source as IDictionary<string, object>).ContainsKey(key))
        {
            return null;
        }

        if (source.GetField(key) is not DateTime result)
        {
            throw new InvalidOperationException($"The field under '{key}' was not {typeof(DateTime).FullName}");
        }

        return result;
    }

    /// <summary>
    /// Extension to retrieve a list of Expando Object values filtered by key.
    /// </summary>
    /// <param name="source">Object to be queried.</param>
    /// <param name="key">The key for the field to be retrieved.</param>
    /// <returns>A list of Expando Object values filtered by key.</returns>
    public static IList<object> GetList(this ExpandoObject source, string key)
    {
        IDictionary<string, object> dict = source;

        if (dict[key] is not IList<object> array)
        {
            throw new InvalidOperationException($"The field under '{key}' was not a list type.");
        }

        return array;
    }

    /// <summary>
    /// Extension to retrieve a list of Expando Object values filtered by key, else an empty dictionary of default data types.
    /// </summary>
    /// <param name="source">Object to be queried.</param>
    /// <param name="key">The key for the field to be retrieved.</param>
    /// <returns>A list of Expando Object values filtered by key, else an empty dictionary of default data types.</returns>
    public static IList<object> GetListOrDefault(this ExpandoObject source, string key)
    {
        IDictionary<string, object> dict = source;

        if (dict.TryGetValue(key, out object list) && list is IList<object> typedList)
        {
            return typedList;
        }

        return default;
    }

    /// <summary>
    /// Gets an inner-expando object, else returns the default data type.
    /// </summary>
    /// <param name="source">Object to be queried.</param>
    /// <param name="key">The key for the field to be retrieved.</param>
    /// <returns>An inner-expando object, else returns the default data type.</returns>
    public static ExpandoObject GetExpandoOrDefault(this ExpandoObject source, string key)
    {
        IDictionary<string, object> dict = source;

        if (dict.TryGetValue(key, out object value) && value is ExpandoObject expando)
        {
            return expando;
        }

        return default;
    }

    /// <summary>
    /// Gets a value from an Expando object by it's named key.
    /// </summary>
    /// <param name="source">Object to be queried.</param>
    /// <param name="key">The key for the field to be retrieved.</param>
    /// <typeparam name="T">The field type.</typeparam>
    /// <returns>A value from an Expando object by it's named key.</returns>
    private static T GetFieldValueOrDefault<T>(this ExpandoObject source, string key)
    {
        var dict = source as IDictionary<string, object>;

        if (!dict.TryGetValue(key, out var field))
        {
            return default;
        }

        if (field is long l)
        {
            if (typeof(T) == typeof(long))
            {
                return l is T t ? t : default;
            }

            if (typeof(T) == typeof(int))
            {
                return (int)l is T i ? i : default;
            }

            if (typeof(T) == typeof(short))
            {
                return (short)l is T s ? s : default;
            }
        }

        if (field is not T result)
        {
            return default;
        }

        return result;
    }
}
