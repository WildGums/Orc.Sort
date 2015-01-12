namespace Orc.Sort.Tests.NSort
{
    using System;
    using System.Collections;

    using NUnit.Framework;

    using Orc.Sort.Interfaces;
    using Orc.Sort.NSort;

#if (SILVERLIGHT)
    using C5;
#endif

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
#if (SILVERLIGHT)
            var sortedList = new TreeDictionary<int, object>();
#else
            var sortedList = new SortedList();
#endif

            foreach (int key in list)
            {
                sortedList.Add(key, null);
            }

            // sort table
            this.Sorter.Sort(list);

            Assert.AreEqual(sortedList.Keys, list);
        }
    }

    [TestFixture]
    public class QuickSorterTest : SorterTest
    {
        [SetUp]
        public void SetUp()
        {
            this.Sorter = new QuickSorter();
        }

        [Test]
        public void Test()
        {
            this.SortTest();
        }
    }


    [TestFixture]
    public class BubbleSorterTest : SorterTest
    {
        [SetUp]
        public void SetUp()
        {
            this.Sorter = new BubbleSorter();
        }

        [Test]
        public void Test()
        {
            this.SortTest();
        }
    }


    [TestFixture]
    public class BiDirectionalBubbleSortTest : SorterTest
    {
        [SetUp]
        public void SetUp()
        {
            this.Sorter = new BiDirectionalBubbleSort();
        }

        [Test]
        public void Test()
        {
            this.SortTest();
        }
    }


    [TestFixture]
    public class ComboSort11Test : SorterTest
    {
        [SetUp]
        public void SetUp()
        {
            this.Sorter = new ComboSort11();
        }

        [Test]
        public void Test()
        {
            this.SortTest();
        }
    }

    [TestFixture]
    public class HeapSortTest : SorterTest
    {
        [SetUp]
        public void SetUp()
        {
            this.Sorter = new HeapSort();
        }

        [Test]
        public void Test()
        {
            this.SortTest();
        }
    }

    [TestFixture]
    //public class ShearSorterTest : SorterTest
    //{
    //    [SetUp]
    //    public void SetUp()
    //    {
    //        this.Sorter = new ShearSorter();
    //    }

    //    [Test]
    //    public void Test()
    //    {
    //        this.SortTest();
    //    }
    //}


    [TestFixture]
    public class OddEvenTransportSorterTest : SorterTest
    {
        [SetUp]
        public void SetUp()
        {
            this.Sorter = new OddEvenTransportSorter();
        }

        [Test]
        public void Test()
        {
            this.SortTest();
        }
    }

    [TestFixture]
    public class FastQuickSorterTest : SorterTest
    {
        [SetUp]
        public void SetUp()
        {
            this.Sorter = new FastQuickSorter();
        }

        [Test]
        public void Test()
        {
            this.SortTest();
        }
    }

    [TestFixture]
    public class SelectionSortTest : SorterTest
    {
        [SetUp]
        public void SetUp()
        {
            this.Sorter = new SelectionSort();
        }

        [Test]
        public void Test()
        {
            this.SortTest();
        }
    }

    [TestFixture]
    public class ShakerTest : SorterTest
    {
        [SetUp]
        public void SetUp()
        {
            this.Sorter = new ShakerSort();
        }

        [Test]
        public void Test()
        {
            this.SortTest();
        }
    }

    [TestFixture]
    public class DoubleStorageMergeSortTest : SorterTest
    {
        [SetUp]
        public void SetUp()
        {
            this.Sorter = new DoubleStorageMergeSort();
        }

        [Test]
        public void Test()
        {
            this.SortTest();
        }
    }

    [TestFixture]
    public class InPlaceMergeSortTest : SorterTest
    {
        [SetUp]
        public void SetUp()
        {
            this.Sorter = new InPlaceMergeSort();
        }

        [Test]
        public void Test()
        {
            this.SortTest();
        }
    }

    [TestFixture]
    public class InsertionSortTest : SorterTest
    {
        [SetUp]
        public void SetUp()
        {
            this.Sorter = new InsertionSort();
        }

        [Test]
        public void Test()
        {
            this.SortTest();
        }
    }

    [TestFixture]
    public class ShellSortTest : SorterTest
    {
        [SetUp]
        public void SetUp()
        {
            this.Sorter = new ShellSort();
        }

        [Test]
        public void Test()
        {
            this.SortTest();
        }
    }

    [TestFixture]
    public class QuickSortWithBubbleSortTest : SorterTest
    {
        [SetUp]
        public void SetUp()
        {
            this.Sorter = new QuickSortWithBubbleSort();
        }

        [Test]
        public void Test()
        {
            this.SortTest();
        }
    }
}
