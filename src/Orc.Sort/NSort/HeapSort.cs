namespace Orc.Sort.NSort
{
    using System.Collections;

    public class HeapSort : SwapSorter
    {
        #region Constructors
        public HeapSort()
        {
        }

        public HeapSort(IComparer comparer, ISwap swapper)
            : base(comparer, swapper)
        {
        }
        #endregion

        #region Methods
        public override void Sort(IList list)
        {
            int i;

            var n = list.Count;
            for (i = n / 2; i > 0; i--)
            {
                DownHeap(list, i, n);
            }

            do
            {
                Swapper.Swap(list, 0, n - 1);
                n = n - 1;
                DownHeap(list, 1, n);
            } while (n > 1);
        }

        private void DownHeap(IList list, int k, int n)
        {
            var loop = true;

            while (k <= n / 2 && loop)
            {
                var j = k + k;
                if (j < n)
                {
                    if (Comparer.Compare(list[j - 1], list[j]) < 0)
                    {
                        j++;
                    }
                }

                if (Comparer.Compare(list[k - 1], list[j - 1]) >= 0)
                {
                    loop = false;
                }
                else
                {
                    Swapper.Swap(list, k - 1, j - 1);
                    k = j;
                }
            }
        }
        #endregion
    }
}
