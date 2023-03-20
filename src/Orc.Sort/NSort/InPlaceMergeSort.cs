namespace Orc.Sort.NSort;

using System;
using System.Collections;

public class InPlaceMergeSort : SwapSorter
{
    public InPlaceMergeSort()
    {
    }

    public InPlaceMergeSort(IComparer comparer, ISwap swapper)
        : base(comparer, swapper)
    {
    }

    public override void Sort(IList list)
    {
        ArgumentNullException.ThrowIfNull(list);

        Sort(list, 0, list.Count - 1);
    }

    private void Sort(IList list, int fromPos, int toPos)
    {
        ArgumentNullException.ThrowIfNull(list);

        if (fromPos >= toPos)
        {
            return;
        }

        var mid = (fromPos + toPos) / 2;

        Sort(list, fromPos, mid);
        Sort(list, mid + 1, toPos);

        var endLow = mid;
        var startHigh = mid + 1;

        while (fromPos <= endLow & startHigh <= toPos)
        {
            if (Comparer.Compare(list[fromPos], list[startHigh]) < 0)
            {
                fromPos++;
            }
            else
            {
                var tmp = list[startHigh];
                int i;
                for (i = startHigh - 1; i >= fromPos; i--)
                {
                    Swapper.Set(list, i + 1, list[i]);
                }

                Swapper.Set(list, fromPos, tmp);
                fromPos++;
                endLow++;
                startHigh++;
            }
        }
    }
}