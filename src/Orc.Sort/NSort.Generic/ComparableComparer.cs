namespace Orc.Sort.NSort.Generic;

using System;
using System.Collections.Generic;

/// <summary>
/// Default <see cref="IComparable"/> object comparer.
/// </summary>
public class ComparableComparer<T> : IComparer<T>
{
    int IComparer<T>.Compare(T? x, T? y)
    {
        return Compare((IComparable<T>?) x, y);
    }

    public int Compare(IComparable<T>? x, T? y)
    {
        return x switch
        {
            null when y is null => 0,
            null => -1,
            _ => y is null ? 1 : x.CompareTo(y)
        };
    }
}
