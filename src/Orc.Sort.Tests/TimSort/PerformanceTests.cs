#region Header
// --------------------------------------------------------------------------------------
// TimSortTests.TimSortTests.cs
// --------------------------------------------------------------------------------------
// 
// 
//
// Copyright (c) 2011 Sepura Plc 
//
// Sepura Confidential
//
// Created: 9/22/2011 9:18:00 AM : SEPURA/krajewskim on SEPURA1051 
// 
// --------------------------------------------------------------------------------------
#endregion

namespace Orc.Sort.Tests.TimSort
{
    using System;
    using System.Collections.Generic;
    using System.Threading;

    using NUnit.Framework;

    using Orc.Sort.TimSort;

    [TestFixture]
    public class PerformanceTests
    {
        const int maxSize = 100000;
        const int seed = 1234;

        public static int Compare(int a, int b)
        {
            // Thread.Sleep(TimeSpan.FromTicks(5)); // make compare function slow
            return a.CompareTo(b);
        }

//The following tests cannot pass in Silverlight 
//because they require elevated trust
#if (!SILVERLIGHT)
        [Test]
        public void RandomTests()
        {
            Console.WriteLine("<<< Random data >>>");

            var r = new Random(seed);
            int[] a = new int[maxSize];
            int[] b = new int[maxSize];

            Console.WriteLine("Preparing...");
            for (int i = 0; i < maxSize; i++) a[i] = b[i] = r.Next();

            Console.WriteLine("Sorting...");
            PerformanceTimer.Debug("builtin", 1, () => Array.Sort(b, Compare), maxSize);
            PerformanceTimer.Debug("timsort", 1, () => a.TimSort(Compare), maxSize);

            Console.WriteLine("Testing...");
            for (int i = 0; i < maxSize; i++) Assert.AreEqual(a[i], b[i]);
        }

        [Test]
        public void SortedTests()
        {
            Console.WriteLine("<<< Generally ascending data >>>");

            var r = new Random(seed);
            int[] a = new int[maxSize];
            int[] b = new int[maxSize];
            int value = int.MinValue;

            Console.WriteLine("Preparing...");
            for (int i = 0; i < maxSize; i++)
            {
                value = 
                    r.Next(100) < 80 
                    ? value + r.Next(100) 
                    : value - r.Next(100);
                a[i] = b[i] = value;
            }

            Console.WriteLine("Sorting...");
            PerformanceTimer.Debug("builtin", 1, () => Array.Sort(b, Compare), maxSize);
            PerformanceTimer.Debug("timsort", 1, () => a.TimSort(Compare), maxSize);

            Console.WriteLine("Testing...");
            for (int i = 0; i < maxSize; i++) Assert.AreEqual(a[i], b[i]);
        }
    
        [Test]
        public void ReversedTests()
        {
            Console.WriteLine("<<< Generally descending data >>>");

            var r = new Random(seed);
            int[] a = new int[maxSize];
            int[] b = new int[maxSize];
            int value = int.MaxValue;

            Console.WriteLine("Preparing...");
            for (int i = 0; i < maxSize; i++)
            {
                value = 
                    r.Next(100) < 80 
                    ? value - r.Next(100) 
                    : value + r.Next(100);
                a[i] = b[i] = value;
            }

            Console.WriteLine("Sorting...");
            PerformanceTimer.Debug("builtin", 1, () => Array.Sort(b, Compare), maxSize);
            PerformanceTimer.Debug("timsort", 1, () => a.TimSort(Compare), maxSize);

            Console.WriteLine("Testing...");
            for (int i = 0; i < maxSize; i++) Assert.AreEqual(a[i], b[i]);
        }

        [Test]
        public void RandomTests_List()
        {
            Console.WriteLine("<<< Random data (buffered List<T>) >>>");

            var r = new Random(seed);
            var a = new List<int>(maxSize);
            int[] b = new int[maxSize];

            Console.WriteLine("Preparing...");
            for (int i = 0; i < maxSize; i++) b[i] = r.Next();
            a.AddRange(b);

            Console.WriteLine("Sorting...");
            PerformanceTimer.Debug("builtin", 1, () => Array.Sort(b, Compare), maxSize);
            PerformanceTimer.Debug("timsort", 1, () => a.TimSort(Compare), maxSize);

            Console.WriteLine("Testing...");
            for (int i = 0; i < maxSize; i++) Assert.AreEqual(a[i], b[i]);
        }
        
        [Test]
        public void SortedTests_List()
        {
            Console.WriteLine("<<< Generally ascending data (buffered List<T>) >>>");

            var r = new Random(seed);
            var a = new List<int>(maxSize);
            int[] b = new int[maxSize];
            int value = int.MinValue;

            Console.WriteLine("Preparing...");
            for (int i = 0; i < maxSize; i++)
            {
                value = 
                    r.Next(100) < 80 
                    ? value + r.Next(100) 
                    : value - r.Next(100);
                b[i] = value;
            }
            a.AddRange(b);

            Console.WriteLine("Sorting...");
            PerformanceTimer.Debug("builtin", 1, () => Array.Sort(b, Compare), maxSize);
            PerformanceTimer.Debug("timsort", 1, () => a.TimSort(Compare), maxSize);

            Console.WriteLine("Testing...");
            for (int i = 0; i < maxSize; i++) Assert.AreEqual(a[i], b[i]);
        }
        
        [Test]
        public void ReversedTests_List()
        {
            Console.WriteLine("<<< Generally descending data (buffered List<T>) >>>");

            var r = new Random(seed);
            var a = new List<int>(maxSize);
            int[] b = new int[maxSize];
            int value = int.MaxValue;

            Console.WriteLine("Preparing...");
            for (int i = 0; i < maxSize; i++)
            {
                value = 
                    r.Next(100) < 80 
                    ? value - r.Next(100) 
                    : value + r.Next(100);
                b[i] = value;
            }
            a.AddRange(b);

            Console.WriteLine("Sorting...");
            PerformanceTimer.Debug("builtin", 1, () => Array.Sort(b, Compare), maxSize);
            PerformanceTimer.Debug("timsort", 1, () => a.TimSort(Compare), maxSize);

            Console.WriteLine("Testing...");
            for (int i = 0; i < maxSize; i++) Assert.AreEqual(a[i], b[i]);
        }
        
        [Test]
        public void RandomTests_ListWithMergeBack()
        {
            Console.WriteLine("<<< Random data (buffered vs non-buffered List<T>) >>>");

            var r = new Random(seed);
            var a = new List<int>(maxSize);
            var b = new List<int>(maxSize);

            Console.WriteLine("Preparing...");
            for (int i = 0; i < maxSize; i++)
            {
                var value = r.Next();
                a.Add(value);
                b.Add(value);
            }

            Console.WriteLine("Sorting...");
            PerformanceTimer.Debug("timsort (non-buffered)", 1, () => a.TimSort(Compare, false), maxSize);
            PerformanceTimer.Debug("timsort (buffered)", 1, () => b.TimSort(Compare, true), maxSize);

            Console.WriteLine("Testing...");
            for (int i = 0; i < maxSize; i++) Assert.AreEqual(a[i], b[i]);
        }
#endif
    }
}
