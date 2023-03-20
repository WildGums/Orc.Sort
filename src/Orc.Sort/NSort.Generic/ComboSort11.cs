namespace Orc.Sort.NSort.Generic;

using System.Collections.Generic;
    

public class ComboSort11<T> : SwapSorter<T>
{
    private const double c_COMBOSORTSHRINKFACTOR = 1.3;

    public ComboSort11() : base()
    {
    }

    public ComboSort11(IComparer<T> comparer, ISwap<T> swapper)
        : base(comparer, swapper)
    {
    }

    public override void Sort(IList<T> list)
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
                gap = (int) (gap/c_COMBOSORTSHRINKFACTOR);
            }

            gap = gap switch
            {
                0 => 1,
                9 => 11,
                10 => 11,
                _ => gap
            };

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
        } 
        while (flipped || (gap > 1));
    }
}
