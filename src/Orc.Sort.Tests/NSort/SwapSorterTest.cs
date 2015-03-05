// --------------------------------------------------------------------------------------------------------------------
// <copyright file="SwapSorterTest.cs" company="Orcomp development team">
//   Copyright (c) 2008 - 2015 Orcomp development team. All rights reserved.
// </copyright>
// --------------------------------------------------------------------------------------------------------------------


namespace Orc.Sort.Tests.NSort
{
    using System;
    using NUnit.Framework;
    using Sort.NSort;

    /// <summary>
    /// Summary description for SwapSorterTest.
    /// </summary>
    [TestFixture]
    public class SwapSorterTest
    {
        #region Properties
        public SwapSorter Sorter
        {
            get { return new BubbleSorter(); }
        }
        #endregion

        #region Methods
        [Test]
        [ExpectedException(typeof (ArgumentNullException))]
        public void NullSwapper()
        {
            SwapSorter sorter = this.Sorter;
            sorter.Swapper = null;
        }

        [Test]
        [ExpectedException(typeof (ArgumentNullException))]
        public void NullComparer()
        {
            SwapSorter sorter = this.Sorter;
            sorter.Comparer = null;
        }
        #endregion
    }
}