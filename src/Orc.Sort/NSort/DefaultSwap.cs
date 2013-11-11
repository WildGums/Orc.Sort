namespace Orc.Sort.NSort
{
    using System.Collections;

    using Orc.Sort.Interfaces;

    /// <summary>
	/// Default swap class
	/// </summary>
	public class DefaultSwap : ISwap
	{
		public void Swap(IList array, int left, int right)
		{
			object swap=array[left];
			array[left]=array[right];
			array[right]=swap;
		}

		public void Set(IList array, int left, int right)
		{
			array[left]=array[right];
		}

		public void Set(IList array, int left, object obj)
		{
			array[left]=obj;
		}
	}
}
