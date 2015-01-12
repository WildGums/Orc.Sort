namespace Orc.Sort.Tests.NSort
{
    using System;

    using NUnit.Framework;

    using Orc.Sort.NSort;

    /// <summary>
    /// Summary description for SwapSorterTest.
    /// </summary>
    [TestFixture]
    public class SwapSorterTest
    {
        public SwapSorter Sorter
        {
            get
            {
                return new BubbleSorter();
            }
        }

        [Test]
        [ExpectedException(typeof(ArgumentNullException))]
        public void NullSwapper()
        {
            SwapSorter sorter = this.Sorter;
            sorter.Swapper = null;
        }

        [Test]
        [ExpectedException(typeof(ArgumentNullException))]
        public void NullComparer()
        {
            SwapSorter sorter = this.Sorter;
            sorter.Comparer = null;
        }
    }
}
