namespace Orc.Sort.Tests.NSort;

using System;
using System.Collections;
    
using NUnit.Framework;
using Sort.NSort;

/// <summary>
/// Summary description for SorterTest.
/// </summary>
public class SorterTest
{
    public SorterTest()
    {
        Sorter = null;
    }

    public ISorter Sorter { get; set; }

    public void SortTest()
    {
        var rnd = new Random();
        var list = new int[1000];
        int i;

        for (i = 0; i < list.Length; ++i)
        {
            list[i] = rnd.Next();
        }

        // create sorted list
        var sortedList = new SortedList();

        foreach (var key in list)
        {
            sortedList.Add(key, null);
        }

        // sort table
        Sorter.Sort(list);

        Assert.That(list, Is.EqualTo(sortedList.Keys));
    }
}

[TestFixture]
public class QuickSorterTest : SorterTest
{
    [SetUp]
    public void SetUp()
    {
        Sorter = new QuickSorter();
    }

    [Test]
    public void Test()
    {
        SortTest();
    }
}

[TestFixture]
public class BubbleSorterTest : SorterTest
{
    [SetUp]
    public void SetUp()
    {
        Sorter = new BubbleSorter();
    }

    [Test]
    public void Test()
    {
        SortTest();
    }
}

[TestFixture]
public class BiDirectionalBubbleSortTest : SorterTest
{
    [SetUp]
    public void SetUp()
    {
        Sorter = new BiDirectionalBubbleSort();
    }

    [Test]
    public void Test()
    {
        SortTest();
    }
}

[TestFixture]
public class ComboSort11Test : SorterTest
{
    [SetUp]
    public void SetUp()
    {
        Sorter = new ComboSort11();
    }

    [Test]
    public void Test()
    {
        SortTest();
    }
}

[TestFixture]
public class HeapSortTest : SorterTest
{
    [SetUp]
    public void SetUp()
    {
        Sorter = new HeapSort();
    }

    [Test]
    public void Test()
    {
        SortTest();
    }
}

[TestFixture]
public class OddEvenTransportSorterTest : SorterTest
{
    [SetUp]
    public void SetUp()
    {
        Sorter = new OddEvenTransportSorter();
    }

    [Test]
    public void Test()
    {
        SortTest();
    }
}

[TestFixture]
public class FastQuickSorterTest : SorterTest
{
    [SetUp]
    public void SetUp()
    {
        Sorter = new FastQuickSorter();
    }

    [Test]
    public void Test()
    {
        SortTest();
    }
}

[TestFixture]
public class SelectionSortTest : SorterTest
{
    [SetUp]
    public void SetUp()
    {
        Sorter = new SelectionSort();
    }

    [Test]
    public void Test()
    {
        SortTest();
    }
}

[TestFixture]
public class ShakerTest : SorterTest
{
    [SetUp]
    public void SetUp()
    {
        Sorter = new ShakerSort();
    }

    [Test]
    public void Test()
    {
        SortTest();
    }
}

[TestFixture]
public class DoubleStorageMergeSortTest : SorterTest
{
    [SetUp]
    public void SetUp()
    {
        Sorter = new DoubleStorageMergeSort();
    }

    [Test]
    public void Test()
    {
        SortTest();
    }
}

[TestFixture]
public class InPlaceMergeSortTest : SorterTest
{
    [SetUp]
    public void SetUp()
    {
        Sorter = new InPlaceMergeSort();
    }

    [Test]
    public void Test()
    {
        SortTest();
    }
}

[TestFixture]
public class InsertionSortTest : SorterTest
{
    [SetUp]
    public void SetUp()
    {
        Sorter = new InsertionSort();
    }

    [Test]
    public void Test()
    {
        SortTest();
    }
}

[TestFixture]
public class ShellSortTest : SorterTest
{
    [SetUp]
    public void SetUp()
    {
        Sorter = new ShellSort();
    }

    [Test]
    public void Test()
    {
        SortTest();
    }
}

[TestFixture]
public class QuickSortWithBubbleSortTest : SorterTest
{
    [SetUp]
    public void SetUp()
    {
        Sorter = new QuickSortWithBubbleSort();
    }

    [Test]
    public void Test()
    {
        SortTest();
    }
}
