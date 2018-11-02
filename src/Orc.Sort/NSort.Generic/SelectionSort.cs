// --------------------------------------------------------------------------------------------------------------------
// <copyright file="SelectionSort.cs" company="WildGums">
//   Copyright (c) 2008 - 2018 WildGums. All rights reserved.
// </copyright>
// --------------------------------------------------------------------------------------------------------------------


namespace Orc.Sort.NSort.Generic
{
    using System.Collections.Generic;

    public class SelectionSort<T> : SwapSorter<T>
    {
        #region Constructors
        public SelectionSort()
        {
        }

        public SelectionSort(IComparer<T> comparer, ISwap<T> swapper)
            : base(comparer, swapper)
        {
        }
        #endregion

        #region Methods
        public override void Sort(IList<T> list)
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
        #endregion
    }
}
