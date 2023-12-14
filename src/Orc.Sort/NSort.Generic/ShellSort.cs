﻿namespace Orc.Sort.NSort.Generic;

using System.Collections.Generic;

public class ShellSort<T> : SwapSorter<T>
{
    public ShellSort()
    {
    }

    public ShellSort(IComparer<T> comparer, ISwap<T> swapper)
        : base(comparer, swapper)
    {
    }

    public override void Sort(IList<T> list)
    {
        var h = 1;

        while (h * 3 + 1 <= list.Count)
        {
            h = 3 * h + 1;
        }

        while (h > 0)
        {
            int i;
            for (i = h - 1; i < list.Count; i++)
            {
                var b = list[i];
                var j = i;
                var loop = true;
                while (loop)
                {
                    if (j >= h)
                    {
                        if (Comparer.Compare(list[j - h], b) > 0)
                        {
                            Swapper.Set(list, j, j - h);
                            j = j - h;
                        }
                        else
                        {
                            loop = false;
                        }
                    }
                    else
                    {
                        loop = false;
                    }
                }

                Swapper.Set(list, j, b);
            }

            h = h / 3;
        }
    }
}