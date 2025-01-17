namespace DfT.DTRO.Extensions;

public static class ExpandoObjectExtensions
{
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

    public static T GetValueOrDefault<T>(this ExpandoObject source, string path)
        => path.Split('.') is { Length: > 1 } split
            ? source.GetValueOrDefault<T>(split)
            : source.GetFieldValueOrDefault<T>(path);

    public static void PutValue<T>(this ExpandoObject source, string path, T value)
    {
        if (path.Split('.') is { Length: > 1 } split)
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

    public static object GetField(this ExpandoObject source, string key)
    {
        var dict = source as IDictionary<string, object>;

        return dict[key];
    }

    public static bool HasField(this ExpandoObject source, string key)
    {
        var dict = source as IDictionary<string, object>;

        return dict.ContainsKey(key);
    }

    public static ExpandoObject GetExpando(this ExpandoObject source, string key)
    {
        if (source.GetField(key) is not ExpandoObject expando)
        {
            throw new InvalidOperationException($"The field under '{key}' was not an expando type.");
        }

        return expando;
    }

    public static T GetValue<T>(this ExpandoObject source, string key)
    {
        if (source.GetField(key) is not T result)
        {
            throw new InvalidOperationException($"The field under '{key}' was not {typeof(T).FullName}");
        }

        return result;
    }

    public static DateTime? GetDateTimeOrNull(this ExpandoObject source, string key)
    {
        if (!(source as IDictionary<string, object>).ContainsKey(key))
        {
            return null;
        }

        if (source.GetField(key) is not DateTime result)
        {
            return null;
        }

        return result;
    }

    public static IList<object> GetList(this ExpandoObject source, string key)
    {
        IDictionary<string, object> dict = source;

        if (dict[key] is not IList<object> array)
        {
            throw new InvalidOperationException($"The field under '{key}' was not a list type.");
        }

        return array;
    }

    public static IList<object> GetListOrDefault(this ExpandoObject source, string key)
    {
        IDictionary<string, object> dict = source;

        if (dict.TryGetValue(key, out object list) && list is IList<object> typedList)
        {
            return typedList;
        }

        return new List<object>();
    }

    public static ExpandoObject GetExpandoOrDefault(this ExpandoObject source, string key)
    {
        IDictionary<string, object> dict = source;

        if (dict.TryGetValue(key, out object value) && value is ExpandoObject expando)
        {
            return expando;
        }

        return null;
    }

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

        if (field is double d)
        {
            if (typeof(T) == typeof(double))
            {
                return (double)d is T db ? db : default;
            }

            if (typeof(T) == typeof(decimal))
            {
                return (decimal)d is T dc ? dc : default;
            }
        }

        if (field is not T result)
        {
            return default;
        }

        return result;
    }
}
