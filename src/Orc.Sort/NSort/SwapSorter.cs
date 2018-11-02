// --------------------------------------------------------------------------------------------------------------------
// <copyright file="SwapSorter.cs" company="WildGums">
//   Copyright (c) 2008 - 2018 WildGums. All rights reserved.
// </copyright>
// --------------------------------------------------------------------------------------------------------------------


namespace Orc.Sort.NSort
{
    using System;
    using System.Collections;
    using Catel;

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
        #region Fields
        private IComparer _comparer;
        private ISwap _swapper;
        #endregion

        #region Constructors
        public SwapSorter()
        {
            _comparer = new ComparableComparer();
            _swapper = new DefaultSwap();
        }

        public SwapSorter(IComparer comparer, ISwap swapper)
        {
            Argument.IsNotNull(() => comparer);
            Argument.IsNotNull(() => swapper);

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
        public IComparer Comparer
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
        public ISwap Swapper
        {
            get => _swapper;
            set => _swapper = value ?? throw new ArgumentNullException(nameof(Swapper));
        }
        #endregion

        #region ISorter Members
        public abstract void Sort(IList list);
        #endregion
    }
}
