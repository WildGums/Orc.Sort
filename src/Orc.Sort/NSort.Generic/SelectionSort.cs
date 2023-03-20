namespace Orc.Sort.NSort.Generic;

using System.Collections.Generic;

public class SelectionSort<T> : SwapSorter<T>
{
    public SelectionSort()
    {
    }

    public SelectionSort(IComparer<T> comparer, ISwap<T> swapper)
        : base(comparer, swapper)
    {
    }

    public override void Sort(IList<T> list)
    {
        int i;

        for (i = 0; i < list.Count; i++)
        {
            var min = i;
            int j;
            for (j = i + 1; j < list.Count; j++)
            {
                if (Comparer.Compare(list[j], list[min]) < 0)
                {
                    min = j;
                }
            }

            Swapper.Swap(list, min, i);
        }
    }
}