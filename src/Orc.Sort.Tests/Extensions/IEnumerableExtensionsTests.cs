using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using NUnit.Framework;
using Orc.Sort.Extensions;

namespace Orc.Sort.Tests.Extensions
{
    [TestFixture]
    public class IEnumerableExtensionsTests
    {
        [Test]
        public void MergeSorted_ReturnsCorrectSequence_1()
        {
            var list_1 = new List<int> { 1, 2, 6, 8 };
            var list_2 = new List<int> { 2, 3, 4, 7 };
            var list_3 = new List<int> { 0, 1, 5, 9 };

            var sorted = list_1.Concat(list_2).Concat(list_3).ToList();
            sorted.Sort();

            var result = (new List<IEnumerable<int>> {list_1, list_2, list_3}).MergeSorted().ToList();

            Assert.AreEqual(sorted, result);
        }
    }
}
