// --------------------------------------------------------------------------------------------------------------------
// <copyright file="DoubleStorageMergeSort.cs" company="WildGums">
//   Copyright (c) 2008 - 2018 WildGums. All rights reserved.
// </copyright>
// --------------------------------------------------------------------------------------------------------------------


namespace Orc.Sort.NSort
{
    using System.Collections;

    public class DoubleStorageMergeSort : SwapSorter
    {
        #region Constructors
        public DoubleStorageMergeSort()
        {
        }

        public DoubleStorageMergeSort(IComparer comparer, ISwap swapper)
            : base(comparer, swapper)
        {
        }
        #endregion

        #region Methods
        public override void Sort(IList list)
        {
            var scratch = new object[list.Count];
            Sort(list, 0, list.Count - 1, scratch);
        }

        private void Sort(IList list, int fromPos, int toPos, object[] scratch)
        {
            if (fromPos >= toPos)
            {
                return;
            }

            var mid = (fromPos + toPos) / 2;
            Sort(list, fromPos, mid, scratch);
            Sort(list, mid + 1, toPos, scratch);

            var tLow = fromPos;
            var tHigh = mid + 1;
            int i;
            for (i = fromPos; i <= toPos; i++)
            {
                if (tLow <= mid)
                {
                    if (tHigh > toPos)
                    {
                        scratch[i] = list[tLow];
                        tLow++;
                    }
                    else
                    {
                        if (Comparer.Compare(list[tLow], list[tHigh]) < 0)
                        {
                            scratch[i] = list[tLow];
                            tLow++;
                        }
                        else
                        {
                            scratch[i] = list[tHigh];
                            tHigh++;
                        }
                    }
                }
                else
                {
                    scratch[i] = list[tHigh];
                    tHigh++;
                }
            }

            for (i = fromPos; i <= toPos; i++)
            {
                Swapper.Set(list, i, scratch[i]);
            }
        }
        #endregion
    }
}
