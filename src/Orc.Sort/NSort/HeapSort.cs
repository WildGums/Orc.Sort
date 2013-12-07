namespace Orc.Sort.NSort
{
    using System.Collections;

    using Orc.Sort.Interfaces;

    public class HeapSort : SwapSorter
	{
		public HeapSort() : base() {}

		public HeapSort(IComparer comparer, ISwap swapper)
			:base(comparer,swapper)
		{
		}

		public override void Sort(IList list) 
		{
			int n;
			int i;

			n = list.Count;
			for (i = n / 2;i>0;i--) 
			{
				this.DownHeap(list, i, n);
			}
			do 
			{
				this.Swapper.Swap(list, 0, n-1);
				n = n - 1;
				this.DownHeap(list, 1, n);
			} while (n>1);
		}

		private void DownHeap(IList list, int k, int n)
		{
			int j;
			bool loop=true;

			while ( (k <= n / 2) && loop) 
			{
				j = k + k;
				if (j < n) 
				{
					if (this.Comparer.Compare(list[j-1], list[j]) < 0) 
					{					
						j++;
					}
				}	
				if (this.Comparer.Compare(list[k-1], list[j-1]) >= 0) 
				{
					loop=false;
				} 
				else 
				{
					this.Swapper.Swap(list, k-1, j-1);
					k = j;
				}
			}
		}
	}
}
