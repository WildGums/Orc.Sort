// --------------------------------------------------------------------------------------------------------------------
// <copyright file="BidirectionalBubbleSort.cs" company="Orcomp development team">
//   Copyright (c) 2008 - 2015 Orcomp development team. All rights reserved.
// </copyright>
// --------------------------------------------------------------------------------------------------------------------


namespace Orc.Sort.NSort
{
    using System.Collections;
    

    public class BiDirectionalBubbleSort : SwapSorter
    {
        #region Constructors
        public BiDirectionalBubbleSort() : base()
        {
        }

        public BiDirectionalBubbleSort(IComparer comparer, ISwap swapper)
            : base(comparer, swapper)
        {
        }
        #endregion

        #region Methods
        public override void Sort(IList list)
        {
            int j;
            int limit;
            int st;
            bool flipped;

            st = -1;
            limit = list.Count;
            flipped = true;
            while (st < limit & flipped)
            {
                flipped = false;
                st++;
                limit--;
                for (j = st; j < limit; j++)
                {
                    if (this.Comparer.Compare(list[j], list[j + 1]) > 0)
                    {
                        this.Swapper.Swap(list, j, j + 1);
                        flipped = true;
                    }
                }

                if (flipped)
                {
                    for (j = limit - 1; j >= st; j--)
                    {
                        if (this.Comparer.Compare(list[j], list[j + 1]) > 0)
                        {
                            this.Swapper.Swap(list, j, j + 1);
                            flipped = true;
                        }
                    }
                }
            }
        }
        #endregion
    }
}