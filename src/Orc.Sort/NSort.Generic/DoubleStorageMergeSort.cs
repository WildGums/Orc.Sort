namespace Orc.Sort.NSort.Generic;

using System.Collections.Generic;
    

public class DoubleStorageMergeSort<T> : SwapSorter<T>
{
    public DoubleStorageMergeSort()
    {
    }

    public DoubleStorageMergeSort(IComparer<T> comparer, ISwap<T> swapper)
        : base(comparer, swapper)
    {
    }

    public override void Sort(IList<T> list)
    {
        var scratch = new T[list.Count];
        Sort(list, 0, list.Count - 1, scratch);
    }

    private void Sort(IList<T> list, int fromPos, int toPos, IList<T> scratch)
    {
        if (fromPos >= toPos)
        {
            return;
        }

        var mid = (fromPos + toPos)/2;
        Sort(list, fromPos, mid, scratch);
        Sort(list, mid + 1, toPos, scratch);

        var tLow = fromPos;
        var tHigh = mid + 1;
        int i;
        for (i = fromPos; i <= toPos; i++)
        {
            if (tLow <= mid)
            {
                if (tHigh > toPos)
                {
                    scratch[i] = list[tLow];
                    tLow++;
                }
                else
                {
                    if (Comparer.Compare(list[tLow], list[tHigh]) < 0)
                    {
                        scratch[i] = list[tLow];
                        tLow++;
                    }
                    else
                    {
                        scratch[i] = list[tHigh];
                        tHigh++;
                    }
                }
            }
            else
            {
                scratch[i] = list[tHigh];
                tHigh++;
            }
        }
        for (i = fromPos; i <= toPos; i++)
        {
            Swapper.Set(list, i, scratch[i]);
        }
    }
}
