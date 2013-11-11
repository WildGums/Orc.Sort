namespace Orc.Sort.NSort
{
    using System.Collections;

    using Orc.Sort.Interfaces;

    /// <summary>
	/// Odd-Even Transport sort parralel algorithm
	/// </summary>
	/// <remarks>
	/// <para>
	/// Source: <a href="http://www.cs.rit.edu/~atk/Java/Sorting/sorting.html">
	/// http://www.cs.rit.edu/~atk/Java/Sorting/sorting.html</a>
	/// </para>
	/// </remarks>
	public class OddEvenTransportSorter : SwapSorter
	{
		public OddEvenTransportSorter()
			:base()
		{
		}

		public OddEvenTransportSorter(IComparer comparer, ISwap swapper)
			:base(comparer,swapper)
		{
		}

		public override void Sort(IList list)
		{
			for (int i = 0; i < list.Count/2; ++i ) 
			{
				for (int j = 0; j+1 < list.Count; j += 2) 
				{
					if (this.Comparer.Compare(list[j], list[j+1])>0) 
						this.Swapper.Swap(list,j,j+1);
				}

				for (int j = 1; j+1 < list.Count; j += 2) 
				{
					if (this.Comparer.Compare(list[j],list[j+1])>0) 
						this.Swapper.Swap(list,j,j+1);
				}
			}	
		}
	}
}
