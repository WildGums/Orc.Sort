namespace Orc.Sort.NSort.Generic
{
    using System;
    using System.Collections;
    using System.Collections.Generic;
    using Catel;

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
        #region Fields
        private IComparer<T> _comparer;
        private ISwap<T> _swapper;
        #endregion

        #region Constructors
        public SwapSorter()
        {
            _comparer = new ComparableComparer<T>();
            _swapper = new DefaultSwap<T>();
        }

        public SwapSorter(IComparer<T> comparer, ISwap<T> swapper)
        {
            ArgumentNullException.ThrowIfNull(comparer);
            ArgumentNullException.ThrowIfNull(swapper);

            _comparer = comparer;
            _swapper = swapper;
        }
        #endregion

        #region Properties
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
            get => _comparer;
            set => _comparer = value ?? throw new ArgumentNullException(nameof(Comparer));
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
            get => _swapper;
            set => _swapper = value ?? throw new ArgumentNullException(nameof(Swapper));
        }
        #endregion

        #region ISorter<T> Members
        public abstract void Sort(IList<T> list);
        #endregion
    }
}
