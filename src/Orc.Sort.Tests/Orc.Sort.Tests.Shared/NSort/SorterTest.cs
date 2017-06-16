// --------------------------------------------------------------------------------------------------------------------
// <copyright file="SorterTest.cs" company="WildGums">
//   Copyright (c) 2008 - 2017 WildGums. All rights reserved.
// </copyright>
// --------------------------------------------------------------------------------------------------------------------


namespace Orc.Sort.Tests.NSort
{
    using System;
    using System.Collections;
    
    using NUnit.Framework;
    using Sort.NSort;

#if (SILVERLIGHT)
    using C5;
#endif

    /// <summary>
    /// Summary description for SorterTest.
    /// </summary>
    public class SorterTest
    {
        #region Constructors
        public SorterTest()
        {
            Sorter = null;
        }
        #endregion

        #region Properties
        public ISorter Sorter { get; set; }
        #endregion

        #region Methods
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
        #endregion
    }

    [TestFixture]
    public class QuickSorterTest : SorterTest
    {
        #region Methods
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
        #endregion
    }

    [TestFixture]
    public class BubbleSorterTest : SorterTest
    {
        #region Methods
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
        #endregion
    }

    [TestFixture]
    public class BiDirectionalBubbleSortTest : SorterTest
    {
        #region Methods
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
        #endregion
    }

    [TestFixture]
    public class ComboSort11Test : SorterTest
    {
        #region Methods
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
        #endregion
    }

    [TestFixture]
    public class HeapSortTest : SorterTest
    {
        #region Methods
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
        #endregion
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
        #region Methods
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
        #endregion
    }

    [TestFixture]
    public class FastQuickSorterTest : SorterTest
    {
        #region Methods
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
        #endregion
    }

    [TestFixture]
    public class SelectionSortTest : SorterTest
    {
        #region Methods
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
        #endregion
    }

    [TestFixture]
    public class ShakerTest : SorterTest
    {
        #region Methods
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
        #endregion
    }

    [TestFixture]
    public class DoubleStorageMergeSortTest : SorterTest
    {
        #region Methods
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
        #endregion
    }

    [TestFixture]
    public class InPlaceMergeSortTest : SorterTest
    {
        #region Methods
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
        #endregion
    }

    [TestFixture]
    public class InsertionSortTest : SorterTest
    {
        #region Methods
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
        #endregion
    }

    [TestFixture]
    public class ShellSortTest : SorterTest
    {
        #region Methods
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
        #endregion
    }

    [TestFixture]
    public class QuickSortWithBubbleSortTest : SorterTest
    {
        #region Methods
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
        #endregion
    }
}