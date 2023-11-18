using System.Collections.Generic;

namespace JellyTetris.Utils;

internal static class SetExt
{
    public static void AddRange<T>(this ISet<T> set, IEnumerable<T> range)
    {
        foreach (var item in range)
        {
            set.Add(item);
        }
    }
}
