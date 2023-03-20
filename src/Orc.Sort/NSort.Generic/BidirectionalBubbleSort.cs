namespace Orc.Sort.NSort.Generic;

using System.Collections.Generic;

public class BiDirectionalBubbleSort<T> : SwapSorter<T>
{
    public BiDirectionalBubbleSort()
    {
    }

    public BiDirectionalBubbleSort(IComparer<T> comparer, ISwap<T> swapper)
        : base(comparer, swapper)
    {
    }

    public override void Sort(IList<T> list)
    {
        var st = -1;
        var limit = list.Count;
        var flipped = true;
        while (st < limit & flipped)
        {
            flipped = false;
            st++;
            limit--;
            int j;
            for (j = st; j < limit; j++)
            {
                if (Comparer.Compare(list[j], list[j + 1]) <= 0)
                {
                    continue;
                }

                Swapper.Swap(list, j, j + 1);
                flipped = true;
            }

            if (!flipped)
            {
                continue;
            }

            for (j = limit - 1; j >= st; j--)
            {
                if (Comparer.Compare(list[j], list[j + 1]) > 0)
                {
                    Swapper.Swap(list, j, j + 1);
                }
            }
        }
    }
}