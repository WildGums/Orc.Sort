namespace Orc.Sort.NSort.Generic;

using System.Collections.Generic;
    

/// <summary>
/// Default swap class
/// </summary>
public class DefaultSwap<T> : ISwap<T>
{
    public void Swap(IList<T> array, int left, int right)
    {
        (array[left], array[right]) = (array[right], array[left]);
    }

    public void Set(IList<T> array, int left, int right)
    {
        array[left] = array[right];
    }

    public void Set(IList<T> array, int left, T obj)
    {
        array[left] = obj;
    }
}
