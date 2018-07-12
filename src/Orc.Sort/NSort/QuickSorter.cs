// --------------------------------------------------------------------------------------------------------------------
// <copyright file="QuickSorter.cs" company="WildGums">
//   Copyright (c) 2008 - 2017 WildGums. All rights reserved.
// </copyright>
// --------------------------------------------------------------------------------------------------------------------


namespace Orc.Sort.NSort
{
    using System.Collections;
    

    /// <summary>
    /// http://www.codeproject.com/csharp/csquicksort.asp
    /// </summary>
    public class QuickSorter : SwapSorter
    {
        #region Constructors
        public QuickSorter()
            : base()
        {
        }

        public QuickSorter(IComparer comparer, ISwap swapper)
            : base(comparer, swapper)
        {
        }
        #endregion

        #region Methods
        /// <summary>
        /// Sorts the array.
        /// </summary>
        /// <param name="array">The array to sort.</param>
        public override void Sort(IList array)
        {
            this.Sort(array, 0, array.Count - 1);
        }

        public void Sort(IList array, int lower, int upper)
        {
            // Check for non-base case
            if (lower < upper)
            {
                // Split and sort partitions
                int split = this.Pivot(array, lower, upper);
                this.Sort(array, lower, split - 1);
                this.Sort(array, split + 1, upper);
            }
        }

        #region Internal
        internal int Pivot(IList array, int lower, int upper)
        {
            // Pivot with first element
            int left = lower + 1;
            object pivot = array[lower];
            int right = upper;

            // Partition array elements
            while (left <= right)
            {
                // Find item out of place
                while ((left <= right) && (this.Comparer.Compare(array[left], pivot) <= 0))
                {
                    ++left;
                }

                while ((left <= right) && (this.Comparer.Compare(array[right], pivot) > 0))
                {
                    --right;
                }

                // Swap values if necessary
                if (left < right)
                {
                    this.Swapper.Swap(array, left, right);
                    ++left;
                    --right;
                }
            }

            // Move pivot element
            this.Swapper.Swap(array, lower, right);
            return right;
        }
        #endregion

        #endregion
    }
}