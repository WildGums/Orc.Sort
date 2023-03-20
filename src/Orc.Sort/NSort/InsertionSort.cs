namespace Orc.Sort.NSort;

using System.Collections;

public class InsertionSort : SwapSorter
{
    public InsertionSort()
    {
    }

    public InsertionSort(IComparer comparer, ISwap swapper)
        : base(comparer, swapper)
    {
    }

    public override void Sort(IList list)
    {
        int i;

        for (i = 1; i < list.Count; i++)
        {
            var j = i;
            var b = list[i];
            while (j > 0 && (Comparer.Compare(list[j - 1], b) > 0))
            {
                Swapper.Set(list, j, list[j - 1]);
                --j;
            }

            Swapper.Set(list, j, b);
        }
    }
}