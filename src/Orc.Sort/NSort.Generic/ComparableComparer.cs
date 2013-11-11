namespace Orc.Sort.NSort.Generic
{
    using System;
    using System.Collections.Generic;

    /// <summary>
	/// Default <see cref="IComparable"/> object comparer.
	/// </summary>
	public class ComparableComparer<T> : IComparer<T>
	{
		public int Compare(IComparable<T> x, T y)
		{
			return x.CompareTo(y);
		}

        #region IComparer Members
        int IComparer<T>.Compare(T x, T y)
        {
            return this.Compare((IComparable<T>)x,y);
        }

        #endregion
	}
}
