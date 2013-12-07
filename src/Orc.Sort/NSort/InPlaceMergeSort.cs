namespace Orc.Sort.NSort
{
    using System.Collections;

    using Orc.Sort.Interfaces;

    public class InPlaceMergeSort : SwapSorter
	{
		public InPlaceMergeSort() : base() {}

		public InPlaceMergeSort(IComparer comparer, ISwap swapper)
			: base(comparer,swapper)
		{}

		public override void Sort(IList list) 
		{
			this.Sort(list, 0, list.Count - 1);
		}

		private void Sort(IList list, int fromPos, int toPos) 
		{
			int end_low;
			int start_high;
			int i;
			object tmp;
			int mid;
			
			if (fromPos < toPos) 
			{
				mid = (fromPos + toPos) / 2;
			
				this.Sort(list, fromPos, mid);
				this.Sort(list, mid + 1, toPos);

				end_low = mid;
				start_high = mid + 1;

				while (fromPos <= end_low & start_high <= toPos) 
				{
					if (this.Comparer.Compare(list[fromPos], list[start_high]) < 0) 
					{
						fromPos++;
					} 
					else 
					{
						tmp = list[start_high];
						for (i = start_high - 1; i >= fromPos; i--) 
						{
							this.Swapper.Set(list, i+1, list[i]);
						}
						this.Swapper.Set(list, fromPos, tmp);
						fromPos++;
						end_low++;
						start_high++;
					}
				}
			}
		}
	}
}