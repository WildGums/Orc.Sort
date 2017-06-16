// --------------------------------------------------------------------------------------------------------------------
// <copyright file="SelectionSort.cs" company="WildGums">
//   Copyright (c) 2008 - 2017 WildGums. All rights reserved.
// </copyright>
// --------------------------------------------------------------------------------------------------------------------


namespace Orc.Sort.NSort
{
    using System.Collections;
    

    public class SelectionSort : SwapSorter
    {
        #region Constructors
        public SelectionSort() : base()
        {
        }

        public SelectionSort(IComparer comparer, ISwap swapper)
            : base(comparer, swapper)
        {
        }
        #endregion

        #region Methods
        public override void Sort(IList list)
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