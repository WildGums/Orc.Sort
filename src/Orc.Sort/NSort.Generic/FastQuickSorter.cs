﻿namespace Orc.Sort.NSort.Generic;

using System.Collections.Generic;
    

/*
* 
* SortAlgorithm.java
*
*
* extended with TriMedian and InsertionSort by Denis Ahrens
* with all the tips from Robert Sedgewick (Algorithms in C++).
* It uses TriMedian and InsertionSort for lists shorts than 4.
* <fuhrmann@cs.tu-berlin.de>
*/

/// <summary>
/// A fast quick sort demonstration algorithm
/// </summary>
/// <remarks>
/// <para>
/// Author James Gosling, Kevin A. Smith, 
/// </para>
/// <para>
/// Source: http://www.cs.ubc.ca/spider/harrison/Java/FastQSortAlgorithm.java.html
/// </para>
/// <para>
/// Ported to C# by Jonathand de Halleux
/// </para>
/// </remarks>
public class FastQuickSorter<T> : SwapSorter<T>
{
    public FastQuickSorter()
        : base()
    {
    }

    public FastQuickSorter(IComparer<T> comparer, ISwap<T> swapper)
        : base(comparer, swapper)
    {
    }

    public override void Sort(IList<T> list)
    {
        QuickSort(list, 0, list.Count - 1);
        InsertionSort(list, 0, list.Count - 1);
    }

    /// <summary>
    /// This is a generic version of C.A.R Hoare's Quick Sort 
    /// algorithm.  This will handle arrays that are already
    /// sorted, and arrays with duplicate keys.
    /// </summary>
    /// <remarks>
    /// If you think of a one dimensional array as going from
    /// the lowest index on the left to the highest index on the right
    /// then the parameters to this function are lowest index or
    /// left and highest index or right.  The first time you call
    /// this function it will be with the parameters 0, a.length - 1.
    /// </remarks>
    /// <param name="list">list to sort</param>
    /// <param name="l">left boundary of array partition</param>
    /// <param name="r">right boundary of array partition</param>
    private void QuickSort(IList<T> list, int l, int r)
    {
        const int m = 4;

        if (r - l <= m)
        {
            return;
        }

        var i = (r + l)/2;
        if (Comparer.Compare(list[l], list[i]) > 0)
        {
            Swapper.Swap(list, l, i); // Tri-Median Method!
        }
        if (Comparer.Compare(list[l], list[r]) > 0)
        {
            Swapper.Swap(list, l, r);
        }
        if (Comparer.Compare(list[i], list[r]) > 0)
        {
            Swapper.Swap(list, i, r);
        }

        var j = r - 1;
        Swapper.Swap(list, i, j);
        i = l;
        var v = list[j];
        for (;;)
        {
            while (Comparer.Compare(list[++i], v) > 0)
            {
            }

            while (Comparer.Compare(list[--j], v) < 0)
            {
            }

            if (j < i)
            {
                break;
            }
            Swapper.Swap(list, i, j);
        }
        Swapper.Swap(list, i, r - 1);
        QuickSort(list, l, j);
        QuickSort(list, i + 1, r);
    }

    private void InsertionSort(IList<T> list, int lo0, int hi0)
    {
        for (var i = lo0 + 1; i <= hi0; i++)
        {
            var v = list[i];
            var j = i;
            while (j > lo0 && (Comparer.Compare(list[j - 1], v) > 0))
            {
                Swapper.Set(list, j, j - 1);
                j--;
            }
            list[j] = v;
        }
    }
}
