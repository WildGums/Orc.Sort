namespace Orc.Sort.NSort.Generic
{
    using System;
    using System.Collections;
    using System.Collections.Generic;

    using Orc.Sort.Interfaces;

    /// <summary>
	/// Abstract base class for Swap sort algorithms.
	/// </summary>
	/// <remarks>
	/// <para>
	/// This class serves as a base class for swap based sort algorithms.
	/// </para>
	/// </remarks>
	public abstract class SwapSorter<T> : ISorter<T>
	{
		private IComparer<T> comparer;
		private ISwap<T> swapper;

		public SwapSorter()
		{
			this.comparer = new ComparableComparer<T>();
			this.swapper = new DefaultSwap<T>();
		}

		public SwapSorter(IComparer<T> comparer, ISwap<T> swapper)
		{
			if (comparer == null)
				throw new ArgumentNullException("comparer");
			if (swapper==null)
				throw new ArgumentNullException("swapper");

			this.comparer = comparer;
			this.swapper = swapper;
		}

		/// <summary>
		/// Gets or sets the <see cref="IComparer"/> object
		/// </summary>
		/// <value>
		/// Comparer object
		/// </value>
		/// <exception cref="ArgumentNullException">
		/// Set property, the value is a null reference
		/// </exception>
		public IComparer<T> Comparer
		{
			get			
			{
				return this.comparer;
			}
			set
			{
				if (value==null)
					throw new ArgumentNullException("comparer");
				this.comparer = value;
			}
		}

		/// <summary>
		/// Gets or set the swapper object
		/// </summary>
		/// <value>
		/// The <see cref="ISwap"/> swapper.
		/// </value>
		/// <exception cref="ArgumentNullException">Swapper is a null reference</exception>
		public ISwap<T> Swapper
		{
			get
			{
				return this.swapper;
			}
			set
			{
				if (value==null)
					throw new ArgumentNullException("swapper");
				this.swapper = value;
			}
		}

		public abstract void Sort(IList<T> list);
	}
}
