namespace Orc.Sort.NSort.Generic
{
    using System.Collections.Generic;

    using Orc.Sort.Interfaces;

    public class ComboSort11<T> : SwapSorter<T>
	{
		private const double c_COMBOSORTSHRINKFACTOR = 1.3;

		public ComboSort11() : base() {}

        public ComboSort11(IComparer<T> comparer, ISwap<T> swapper)
			:base(comparer,swapper)
		{
		}

        public override void Sort(IList<T> list) 
		{
			bool flipped=true;
			int gap;
			int top;
			int i;
			int j;

			gap = list.Count;

			do
			{
				if (gap==2) 
				{
					gap = 1;
				} 
				else 
				{
					gap = (int)(gap / c_COMBOSORTSHRINKFACTOR);
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
				top =list.Count - gap;
				for (i = 0;i<top;i++) 
				{
					j = i + gap;
					if (this.Comparer.Compare(list[i],list[j])>0) 
					{
						this.Swapper.Swap(list, i, j);
						flipped = true;
					}
				}
			} while (flipped || (gap>1) );
		}
	}
}
