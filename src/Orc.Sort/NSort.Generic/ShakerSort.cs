namespace Orc.Sort.NSort.Generic
{
    using System.Collections.Generic;

    using Orc.Sort.Interfaces;

    public class ShakerSort<T> : SwapSorter<T>
	{
		public ShakerSort() : base() {}

        public ShakerSort(IComparer<T> comparer, ISwap<T> swapper)
			:base(comparer,swapper)
		{
		}

        public override void Sort(IList<T> list) 
		{
			int i;
			int j;
			int k;
			int min;
			int max;

			i = 0;
			k = list.Count-1;
			while (i < k) 
			{
				min = i;
				max = i;
				for (j=i+1;j<=k;j++) 
				{
					if (this.Comparer.Compare(list[j], list[min])<0) 
					{
						min = j;
					}
					if (this.Comparer.Compare(list[j], list[max])>0) 
					{
						max = j;
					}
				}
				this.Swapper.Swap(list, min, i);
				if (max == i) 
				{
					this.Swapper.Swap(list, min,k);
				} 
				else 
				{
					this.Swapper.Swap(list, max, k);
				}
				i++;
				k--;
			}
		}
	}
}
