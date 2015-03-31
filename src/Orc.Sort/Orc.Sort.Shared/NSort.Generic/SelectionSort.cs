// --------------------------------------------------------------------------------------------------------------------
// <copyright file="SelectionSort.cs" company="Orcomp development team">
//   Copyright (c) 2008 - 2015 Orcomp development team. All rights reserved.
// </copyright>
// --------------------------------------------------------------------------------------------------------------------


namespace Orc.Sort.NSort.Generic
{
    using System.Collections.Generic;
    

    public class SelectionSort<T> : SwapSorter<T>
    {
        #region Constructors
        public SelectionSort() : base()
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
            int j;
            int min;

            for (i = 0; i < list.Count; i++)
            {
                min = i;
                for (j = i + 1; j < list.Count; j++)
                {
                    if (this.Comparer.Compare(list[j], list[min]) < 0)
                    {
                        min = j;
                    }
                }
                this.Swapper.Swap(list, min, i);
            }
        }
        #endregion
    }
}