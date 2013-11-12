using System;
using NUnit.Framework;
using System.Linq;
using System.Collections.Generic;
using Performance;

namespace TimSort.Tests
{
	[TestFixture]
	public class PerformanceTests
	{
	    private const int maxMem = int.MaxValue/64;
        const int maxSize = maxMem / sizeof(int);
		const int seed = 1234;

		public static int Compare(int a, int b)
		{
			// Thread.Sleep(0); // make compare function slow
			return a.CompareTo(b);
		}

        [Test]
        public void RandomTests_NativeInt32()
        {
            Console.WriteLine("<<< Random data >>>");

            var r = new Random(seed);
            int[] a = new int[maxSize];
            int[] b = new int[maxSize];

            Console.WriteLine("Preparing...");
            for (int i = 0; i < maxSize; i++) a[i] = b[i] = r.Next();

            Console.WriteLine("Sorting...");
            PerformanceTimer.Debug("builtin", 1, () => Array.Sort(b), maxSize);
            PerformanceTimer.Debug("timsort", 1, () => a.TimSort(), maxSize);

            Console.WriteLine("Testing...");
            for (int i = 0; i < maxSize; i++) Assert.AreEqual(a[i], b[i]);
        }

        [Test]
        public void RandomTests_NativeGuid()
        {
            Console.WriteLine("<<< Random data >>>");

            var length = maxMem/16;
            var r = new Random(seed);
            Guid[] a = new Guid[length];
            Guid[] b = new Guid[length];

            Console.WriteLine("Preparing...");
            for (int i = 0; i < length; i++) a[i] = b[i] = Guid.NewGuid();

            Console.WriteLine("Sorting...");
            PerformanceTimer.Debug("builtin", 1, () => Array.Sort(b), maxSize);
            PerformanceTimer.Debug("timsort", 1, () => a.TimSort(), maxSize);

            Console.WriteLine("Testing...");
            for (int i = 0; i < length; i++) Assert.AreEqual(a[i], b[i]);
        }

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
	}
}
