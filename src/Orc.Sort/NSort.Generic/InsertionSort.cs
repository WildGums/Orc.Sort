namespace Orc.Sort.NSort.Generic;

using System.Collections.Generic;
    

public class InsertionSort<T> : SwapSorter<T>
{
    public InsertionSort()
    {
    }

    public InsertionSort(IComparer<T> comparer, ISwap<T> swapper)
        : base(comparer, swapper)
    {
    }

    public override void Sort(IList<T> list)
    {
        for (var i = 1; i < list.Count; i++)
        {
            var j = i;
            var b = list[i];
            while (j > 0 && Comparer.Compare(list[j - 1], b) > 0)
            {
                Swapper.Set(list, j, list[j - 1]);
                --j;
            }
            Swapper.Set(list, j, b);
        }
    }
}
