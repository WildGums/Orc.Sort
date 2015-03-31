// --------------------------------------------------------------------------------------------------------------------
// <copyright file="DoubleStorageMergeSort.cs" company="Orcomp development team">
//   Copyright (c) 2008 - 2015 Orcomp development team. All rights reserved.
// </copyright>
// --------------------------------------------------------------------------------------------------------------------


namespace Orc.Sort.NSort
{
    using System;
    using System.Collections;
    

    public class DoubleStorageMergeSort : SwapSorter
    {
        #region Constructors
        public DoubleStorageMergeSort() : base()
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
            object[] scratch = new IComparable[list.Count];
            this.Sort(list, 0, list.Count - 1, scratch);
        }

        private void Sort(IList list, int fromPos, int toPos, object[] scratch)
        {
            int mid = 0;
            int i;
            int t_low;
            int t_high;

            if (fromPos < toPos)
            {
                mid = (fromPos + toPos)/2;
                this.Sort(list, fromPos, mid, scratch);
                this.Sort(list, mid + 1, toPos, scratch);

                t_low = fromPos;
                t_high = mid + 1;
                for (i = fromPos; i <= toPos; i++)
                {
                    if (t_low <= mid)
                    {
                        if (t_high > toPos)
                        {
                            scratch[i] = list[t_low];
                            t_low++;
                        }
                        else
                        {
                            if (this.Comparer.Compare(list[t_low], list[t_high]) < 0)
                            {
                                scratch[i] = list[t_low];
                                t_low++;
                            }
                            else
                            {
                                scratch[i] = list[t_high];
                                t_high++;
                            }
                        }
                    }
                    else
                    {
                        scratch[i] = list[t_high];
                        t_high++;
                    }
                }
                for (i = fromPos; i <= toPos; i++)
                {
                    this.Swapper.Set(list, i, scratch[i]);
                }
            }
        }
        #endregion
    }
}