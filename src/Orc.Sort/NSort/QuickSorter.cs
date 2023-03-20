namespace Orc.Sort.NSort;

using System.Collections;

/// <summary>
/// http://www.codeproject.com/csharp/csquicksort.asp
/// </summary>
public class QuickSorter : SwapSorter
{
    public QuickSorter()
        : base()
    {
    }

    public QuickSorter(IComparer comparer, ISwap swapper)
        : base(comparer, swapper)
    {
    }

    /// <summary>
    /// Sorts the array.
    /// </summary>
    /// <param name="array">The array to sort.</param>
    public override void Sort(IList array)
    {
        Sort(array, 0, array.Count - 1);
    }

    public void Sort(IList array, int lower, int upper)
    {
        // Check for non-base case
        if (lower >= upper)
        {
            return;
        }

        // Split and sort partitions
        var split = Pivot(array, lower, upper);
        Sort(array, lower, split - 1);
        Sort(array, split + 1, upper);
    }

    internal int Pivot(IList array, int lower, int upper)
    {
        // Pivot with first element
        var left = lower + 1;
        var pivot = array[lower];
        var right = upper;

        // Partition array elements
        while (left <= right)
        {
            // Find item out of place
            while ((left <= right) && (Comparer.Compare(array[left], pivot) <= 0))
            {
                ++left;
            }

            while ((left <= right) && (Comparer.Compare(array[right], pivot) > 0))
            {
                --right;
            }

            // Swap values if necessary
            if (left < right)
            {
                Swapper.Swap(array, left, right);
                ++left;
                --right;
            }
        }

        // Move pivot element
        Swapper.Swap(array, lower, right);
        return right;
    }
}
