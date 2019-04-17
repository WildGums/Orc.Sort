// --------------------------------------------------------------------------------------------------------------------
// <copyright file="ShellSort.cs" company="WildGums">
//   Copyright (c) 2008 - 2018 WildGums. All rights reserved.
// </copyright>
// --------------------------------------------------------------------------------------------------------------------


namespace Orc.Sort.NSort
{
    using System.Collections;

    public class ShellSort : SwapSorter
    {
        #region Constructors
        public ShellSort()
        {
        }

        public ShellSort(IComparer comparer, ISwap swapper)
            : base(comparer, swapper)
        {
        }
        #endregion

        #region Methods
        public override void Sort(IList list)
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
        #endregion
    }
}
