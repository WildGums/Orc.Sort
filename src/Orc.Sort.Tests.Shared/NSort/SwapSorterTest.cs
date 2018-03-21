// --------------------------------------------------------------------------------------------------------------------
// <copyright file="SwapSorterTest.cs" company="WildGums">
//   Copyright (c) 2008 - 2017 WildGums. All rights reserved.
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
        public void NullSwapper()
        {
            Assert.Throws<ArgumentNullException>(() =>
            {
                var sorter = this.Sorter;
                sorter.Swapper = null;
            });
        }

        [Test]
        public void NullComparer()
        {
            Assert.Throws<ArgumentNullException>(() =>
            {
                var sorter = this.Sorter;
                sorter.Comparer = null;
            });
        }
        #endregion
    }
}