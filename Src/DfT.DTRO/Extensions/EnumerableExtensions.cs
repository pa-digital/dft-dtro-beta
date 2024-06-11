using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Reflection;

namespace DfT.DTRO.Extensions;

public static class EnumerableExtensions
{
    public static IEnumerable<(T, T)> Pairs<T>(this IEnumerable<T> source)
    {
        var i = 1;
        foreach (var left in source)
        {
            foreach (T right in source.Skip(i))
            {
                yield return (left, right);
            }

            i++;
        }
    }

    public static IEnumerable<string> GetDisplayName<T>(this Type type) where T : Enum
    {
        T[] enums = (T[])Enum.GetValues(type);
        return enums.Select(it=>it.GetDisplayName()).ToList();
    }

    private static string GetDisplayName(this Enum enumToDisplay) => 
        enumToDisplay.GetAttribute<DisplayAttribute>().Name;

    private static TAttribute GetAttribute<TAttribute>(this Enum enumValue) where TAttribute : Attribute => 
        enumValue
            .GetType()
            .GetMember(enumValue.ToString())
            .First()
            .GetCustomAttribute<TAttribute>();
}
