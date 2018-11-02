// --------------------------------------------------------------------------------------------------------------------
// <copyright file="ShakerSort.cs" company="WildGums">
//   Copyright (c) 2008 - 2018 WildGums. All rights reserved.
// </copyright>
// --------------------------------------------------------------------------------------------------------------------


namespace Orc.Sort.NSort
{
    using System.Collections;

    public class ShakerSort : SwapSorter
    {
        #region Constructors
        public ShakerSort()
        {
        }

        public ShakerSort(IComparer comparer, ISwap swapper)
            : base(comparer, swapper)
        {
        }
        #endregion

        #region Methods
        public override void Sort(IList list)
        {
            var i = 0;
            var k = list.Count - 1;
            while (i < k)
            {
                var min = i;
                var max = i;
                int j;
                for (j = i + 1; j <= k; j++)
                {
                    if (Comparer.Compare(list[j], list[min]) < 0)
                    {
                        min = j;
                    }

                    if (Comparer.Compare(list[j], list[max]) > 0)
                    {
                        max = j;
                    }
                }

                Swapper.Swap(list, min, i);
                Swapper.Swap(list, max == i ? min : max, k);
                i++;
                k--;
            }
        }
        #endregion
    }
}
