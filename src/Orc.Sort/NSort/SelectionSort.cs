namespace Orc.Sort.NSort;

using System.Collections;

public class SelectionSort : SwapSorter
{
    public SelectionSort()
    {
    }

    public SelectionSort(IComparer comparer, ISwap swapper)
        : base(comparer, swapper)
    {
    }

    public override void Sort(IList list)
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