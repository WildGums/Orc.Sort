namespace Orc.Sort.NSort.Generic
{
    using System.Collections.Generic;

    public class ShakerSort<T> : SwapSorter<T>
    {
        #region Constructors
        public ShakerSort() : base()
        {
        }

        public ShakerSort(IComparer<T> comparer, ISwap<T> swapper)
            : base(comparer, swapper)
        {
        }
        #endregion

        #region Methods
        public override void Sort(IList<T> list)
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
