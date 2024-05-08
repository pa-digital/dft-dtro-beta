using System.Collections.Generic;
using System.Linq;

namespace DfT.DTRO.Extensions;

/// <summary>
/// Provides extension methods for manipulating enumerables.
/// </summary>
public static class EnumerableExtensions
{
    /// <summary>
    /// Enumerates each pair of values from the source enumerable.
    /// </summary>
    /// <typeparam name="T">Type of elements.</typeparam>
    /// <returns>Enumerated pairs.</returns>
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
}
