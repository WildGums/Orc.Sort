namespace Orc.Sort.NSort.Generic
{
    using System.Collections.Generic;

    using Orc.Sort.Interfaces;

    /// <summary>
	/// Default swap class
	/// </summary>
	public class DefaultSwap<T> : ISwap<T>
	{
		public void Swap(IList<T> array, int left, int right)
		{
			T swap=array[left];
			array[left]=array[right];
			array[right]=swap;
		}

		public void Set(IList<T> array, int left, int right)
		{
			array[left]=array[right];
		}

		public void Set(IList<T> array, int left, T obj)
		{
			array[left]=obj;
		}
	}
}
