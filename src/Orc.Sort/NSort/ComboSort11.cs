namespace Orc.Sort.NSort;

using System.Collections;

public class ComboSort11 : SwapSorter
{
    private const double ComboSortShrinkFactor = 1.3;

    public ComboSort11()
    {
    }

    public ComboSort11(IComparer comparer, ISwap swapper)
        : base(comparer, swapper)
    {
    }

    public override void Sort(IList list)
    {
        var flipped = true;

        var gap = list.Count;

        do
        {
            if (gap == 2)
            {
                gap = 1;
            }
            else
            {
                gap = (int)(gap / ComboSortShrinkFactor);
            }

            switch (gap)
            {
                case 0:
                    gap = 1;
                    break;
                case 9:
                case 10:
                    gap = 11;
                    break;
            }

            flipped = false;
            var top = list.Count - gap;
            int i;
            for (i = 0; i < top; i++)
            {
                var j = i + gap;
                if (Comparer.Compare(list[i], list[j]) <= 0)
                {
                    continue;
                }

                Swapper.Swap(list, i, j);
                flipped = true;
            }
        } while (flipped || gap > 1);
    }
}