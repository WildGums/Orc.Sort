namespace Orc.Sort.NSort.Generic
{
    using System.Collections.Generic;

    using Orc.Sort.Interfaces;

    public class BiDirectionalBubbleSort<T> : SwapSorter<T>
	{
		public BiDirectionalBubbleSort() : base() {}

        public BiDirectionalBubbleSort(IComparer<T> comparer, ISwap<T> swapper)
			:base(comparer,swapper)
		{
		}

        public override void Sort(IList<T> list) 
		{
			int j;
			int limit;
			int st;
			bool flipped;

			st = -1;
			limit = list.Count;
			flipped=true;
			while (st < limit & flipped) 
			{
				flipped = false;
				st++;
				limit--;
				for (j=st;j<limit;j++) 
				{
					if (this.Comparer.Compare(list[j],list[j+1])>0) 
					{
						this.Swapper.Swap(list, j, j+1);
						flipped = true;
					}
				}

				if (flipped) 
				{
					for (j=limit-1;j>=st;j--) 
					{
						if (this.Comparer.Compare(list[j],list[j+1])>0) 
						{
							this.Swapper.Swap(list, j, j+1);
							flipped = true;
						}
					}
				}
			}
		}
	}
}
