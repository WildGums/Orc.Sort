// --------------------------------------------------------------------------------------------------------------------
// <copyright file="InsertionSort.cs" company="WildGums">
//   Copyright (c) 2008 - 2017 WildGums. All rights reserved.
// </copyright>
// --------------------------------------------------------------------------------------------------------------------


namespace Orc.Sort.NSort
{
    using System.Collections;
    

    public class InsertionSort : SwapSorter
    {
        #region Constructors
        public InsertionSort() : base()
        {
        }

        public InsertionSort(IComparer comparer, ISwap swapper)
            : base(comparer, swapper)
        {
        }
        #endregion

        #region Methods
        public override void Sort(IList list)
        {
            int i;
            int j;
            object b;

            for (i = 1; i < list.Count; i++)
            {
                j = i;
                b = list[i];
                while ((j > 0) && (this.Comparer.Compare(list[j - 1], b) > 0))
                {
                    this.Swapper.Set(list, j, list[j - 1]);
                    --j;
                }
                this.Swapper.Set(list, j, b);
            }
        }
        #endregion
    }
}