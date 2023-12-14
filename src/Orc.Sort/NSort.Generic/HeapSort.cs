namespace Orc.Sort.NSort.Generic;

using System.Collections.Generic;

public class HeapSort<T> : SwapSorter<T>
{
    public HeapSort()
    {
    }

    public HeapSort(IComparer<T> comparer, ISwap<T> swapper)
        : base(comparer, swapper)
    {
    }
    
    public override void Sort(IList<T> list)
    {
        var n = list.Count;
        for (var i = n / 2; i > 0; i--)
        {
            DownHeap(list, i, n);
        }

        do
        {
            Swapper.Swap(list, 0, n - 1);
            n = n - 1;
            DownHeap(list, 1, n);
        } 
        while (n > 1);
    }

    private void DownHeap(IList<T> list, int k, int n)
    {
        var loop = true;
        while (k <= n / 2 && loop)
        {
            var j = k + k;
            if (j < n)
            {
                if (Comparer.Compare(list[j - 1], list[j]) < 0)
                {
                    j++;
                }
            }

            if (Comparer.Compare(list[k - 1], list[j - 1]) >= 0)
            {
                loop = false;
            }
            else
            {
                Swapper.Swap(list, k - 1, j - 1);
                k = j;
            }
        }
    }
}
