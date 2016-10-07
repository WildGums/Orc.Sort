// --------------------------------------------------------------------------------------------------------------------
// <copyright file="InPlaceMergeSort.cs" company="WildGums">
//   Copyright (c) 2008 - 2015 WildGums. All rights reserved.
// </copyright>
// --------------------------------------------------------------------------------------------------------------------


namespace Orc.Sort.NSort.Generic
{
    using System.Collections.Generic;
    

    public class InPlaceMergeSort<T> : SwapSorter<T>
    {
        #region Constructors
        public InPlaceMergeSort() : base()
        {
        }

        public InPlaceMergeSort(IComparer<T> comparer, ISwap<T> swapper)
            : base(comparer, swapper)
        {
        }
        #endregion

        #region Methods
        public override void Sort(IList<T> list)
        {
            this.Sort(list, 0, list.Count - 1);
        }

        private void Sort(IList<T> list, int fromPos, int toPos)
        {
            int end_low;
            int start_high;
            int i;
            T tmp;
            int mid;

            if (fromPos < toPos)
            {
                mid = (fromPos + toPos)/2;

                this.Sort(list, fromPos, mid);
                this.Sort(list, mid + 1, toPos);

                end_low = mid;
                start_high = mid + 1;

                while (fromPos <= end_low & start_high <= toPos)
                {
                    if (this.Comparer.Compare(list[fromPos], list[start_high]) < 0)
                    {
                        fromPos++;
                    }
                    else
                    {
                        tmp = list[start_high];
                        for (i = start_high - 1; i >= fromPos; i--)
                        {
                            this.Swapper.Set(list, i + 1, list[i]);
                        }
                        this.Swapper.Set(list, fromPos, tmp);
                        fromPos++;
                        end_low++;
                        start_high++;
                    }
                }
            }
        }
        #endregion
    }
}