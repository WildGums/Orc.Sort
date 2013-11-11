namespace Orc.Sort.NSort.Generic
{
    using System.Collections.Generic;

    using Orc.Sort.Interfaces;

    public class SelectionSort<T> : SwapSorter<T>
	{
		public SelectionSort() : base() {}

        public SelectionSort(IComparer<T> comparer, ISwap<T> swapper)
			:base(comparer,swapper)
		{
		}

        public override void Sort(IList<T> list) 
		{
			int i;
			int j;
			int min;

			for (i=0;i<list.Count;i++) 
			{
				min = i;
				for (j=i+1;j<list.Count;j++) 
				{
					if (this.Comparer.Compare(list[j], list[min])<0) 
					{
						min = j;
					}
				}
				this.Swapper.Swap(list, min, i);
			}
		}
	}
}
