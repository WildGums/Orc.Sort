// --------------------------------------------------------------------------------------------------------------------
// <copyright file="QuickSortWithBubbleSort.cs" company="Orcomp development team">
//   Copyright (c) 2008 - 2015 Orcomp development team. All rights reserved.
// </copyright>
// --------------------------------------------------------------------------------------------------------------------


namespace Orc.Sort.NSort.Generic
{
    using System.Collections.Generic;
    

    public class QuickSortWithBubbleSort<T> : SwapSorter<T>
    {
        #region Constructors
        public QuickSortWithBubbleSort() : base()
        {
        }

        public QuickSortWithBubbleSort(IComparer<T> comparer, ISwap<T> swapper)
            : base(comparer, swapper)
        {
        }
        #endregion

        #region Methods
        public override void Sort(IList<T> list)
        {
            this.Sort1(list, 0, list.Count - 1);
        }

        private void Sort2(IList<T> list, int low, int high)
        {
            int j;
            int i;

            for (j = high; j > low; j--)
            {
                for (i = low; i < j; i++)
                {
                    if (this.Comparer.Compare(list[i], list[i + 1]) > 0)
                    {
                        this.Swapper.Swap(list, i, i + 1);
                    }
                }
            }
        }

        private void Sort1(IList<T> list, int fromPos, int toPos)
        {
            int low;
            int high;
            T pivot;

            low = fromPos;
            high = toPos;
            if (high - low <= 16)
            {
                this.Sort2(list, low, high);
            }
            else
            {
                pivot = list[(low + high)/2];
                list[(low + high)/2] = list[high];
                list[high] = pivot;
                while (low < high)
                {
                    while (this.Comparer.Compare(list[low], pivot) <= 0 & low < high)
                    {
                        low++;
                    }
                    while (this.Comparer.Compare(pivot, list[high]) <= 0 & low < high)
                    {
                        high--;
                    }
                    if (low < high)
                    {
                        this.Swapper.Swap(list, low, high);
                    }
                }
                this.Swapper.Set(list, toPos, high);
                this.Swapper.Set(list, high, pivot);
                this.Sort1(list, fromPos, low - 1);
                this.Sort1(list, high + 1, toPos);
            }
        }
        #endregion
    }
}