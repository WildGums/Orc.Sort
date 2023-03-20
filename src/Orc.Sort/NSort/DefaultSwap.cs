namespace Orc.Sort.NSort;

using System;
using System.Collections;

/// <summary>
/// Default swap class
/// </summary>
public class DefaultSwap : ISwap
{
    public void Swap(IList array, int left, int right)
    {
        ArgumentNullException.ThrowIfNull(array);

        (array[left], array[right]) = (array[right], array[left]);
    }

    public void Set(IList array, int left, int right)
    {
        ArgumentNullException.ThrowIfNull(array);

        array[left] = array[right];
    }

    public void Set(IList array, int left, object? obj)
    {
        ArgumentNullException.ThrowIfNull(array);

        array[left] = obj;
    }
}
