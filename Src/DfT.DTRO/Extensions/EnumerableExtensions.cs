using System.Collections.Generic;
using System.Linq;

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
}
