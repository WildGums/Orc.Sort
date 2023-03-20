namespace Orc.Sort.Tests.NSort;

using System;
using NUnit.Framework;
using Sort.NSort;

/// <summary>
/// Summary description for SwapSorterTest.
/// </summary>
[TestFixture]
public class SwapSorterTest
{
    public SwapSorter Sorter
    {
        get { return new BubbleSorter(); }
    }

    [Test]
    public void NullSwapper()
    {
        Assert.Throws<ArgumentNullException>(() =>
        {
            var sorter = Sorter;
            sorter.Swapper = null;
        });
    }

    [Test]
    public void NullComparer()
    {
        Assert.Throws<ArgumentNullException>(() =>
        {
            var sorter = Sorter;
            sorter.Comparer = null;
        });
    }
}
