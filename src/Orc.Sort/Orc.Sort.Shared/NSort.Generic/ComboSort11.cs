// --------------------------------------------------------------------------------------------------------------------
// <copyright file="ComboSort11.cs" company="WildGums">
//   Copyright (c) 2008 - 2015 WildGums. All rights reserved.
// </copyright>
// --------------------------------------------------------------------------------------------------------------------


namespace Orc.Sort.NSort.Generic
{
    using System.Collections.Generic;
    

    public class ComboSort11<T> : SwapSorter<T>
    {
        #region Fields
        private const double c_COMBOSORTSHRINKFACTOR = 1.3;
        #endregion

        #region Constructors
        public ComboSort11() : base()
        {
        }

        public ComboSort11(IComparer<T> comparer, ISwap<T> swapper)
            : base(comparer, swapper)
        {
        }
        #endregion

        #region Methods
        public override void Sort(IList<T> list)
        {
            bool flipped = true;
            int gap;
            int top;
            int i;
            int j;

            gap = list.Count;

            do
            {
                if (gap == 2)
                {
                    gap = 1;
                }
                else
                {
                    gap = (int) (gap/c_COMBOSORTSHRINKFACTOR);
                }
                switch (gap)
                {
                    case 0:
                        gap = 1;
                        break;
                    case 9:
                        gap = 11;
                        break;
                    case 10:
                        gap = 11;
                        break;
                }
                flipped = false;
                top = list.Count - gap;
                for (i = 0; i < top; i++)
                {
                    j = i + gap;
                    if (this.Comparer.Compare(list[i], list[j]) > 0)
                    {
                        this.Swapper.Swap(list, i, j);
                        flipped = true;
                    }
                }
            } while (flipped || (gap > 1));
        }
        #endregion
    }
}