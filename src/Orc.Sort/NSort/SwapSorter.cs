namespace Orc.Sort.NSort
{
    using System;
    using System.Collections;

    using Orc.Sort.Interfaces;

    /// <summary>
	/// Abstract base class for Swap sort algorithms.
	/// </summary>
	/// <remarks>
	/// <para>
	/// This class serves as a base class for swap based sort algorithms.
	/// </para>
	/// </remarks>
	public abstract class SwapSorter : ISorter
	{
		private IComparer comparer;
		private ISwap swapper;

		public SwapSorter()
		{
			this.comparer = new ComparableComparer();
			this.swapper = new DefaultSwap();
		}

		public SwapSorter(IComparer comparer, ISwap swapper)
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
		public IComparer Comparer
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
		public ISwap Swapper
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

		public abstract void Sort(IList list);
	}
}
