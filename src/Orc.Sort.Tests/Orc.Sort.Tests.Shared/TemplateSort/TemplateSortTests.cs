// --------------------------------------------------------------------------------------------------------------------
// <copyright file="TemplateSortTests.cs" company="WildGums">
//   Copyright (c) 2008 - 2015 WildGums. All rights reserved.
// </copyright>
// --------------------------------------------------------------------------------------------------------------------


namespace Orc.Sort.Tests.TemplateSort
{
    using System;
    using System.Collections.Generic;
    using System.Linq;
    using NUnit.Framework;
    using Sort.TemplateSort;

    [TestFixture]
    public class TemplateSortTests
    {
        #region Methods
        [Test]
        public void SortAccordingTo_EmptySourceCollectionAndTemplateCollection_ReturnsEmptyCollection()
        {
            var sourceCollection = Enumerable.Empty<string>();
            var templateCollection = Enumerable.Empty<string>();

            var result = sourceCollection.SortAccordingTo(templateCollection);

            Assert.AreEqual(sourceCollection, result);
        }

        [Test]
        public void SortAccordingTo_EmptySourceCollection_ReturnsEmptyCollection()
        {
            var sourceCollection = Enumerable.Empty<string>();
            var templateCollection = new[] {"A", "B", "C", "D"};

            var result = sourceCollection.SortAccordingTo(templateCollection);

            Assert.AreEqual(sourceCollection, result);
        }

        [Test]
        public void SortAccordingTo_EmptySourceCollectionWithDuplicatesInTemplateCollection_ReturnsException()
        {
            var sourceCollection = Enumerable.Empty<string>();
            var templateCollection = new[] {"A", "B", "B", "C", "D"};

            Assert.Throws<ArgumentNullException>(() => sourceCollection.SortAccordingTo(templateCollection));
        }

        [Test]
        public void SortAccordingTo_EmptyTemplateCollection_ReturnsSameSourceCollection()
        {
            var sourceCollection = new[] {"A", "B", "C", "D"};
            var templateCollection = Enumerable.Empty<string>();

            var result = sourceCollection.SortAccordingTo(templateCollection);

            Assert.AreEqual(sourceCollection, result);
        }

        [Test]
        public void SortAccordingTo_NullTemplateCollection_ReturnsException()
        {
            var sourceCollection = new[] {"A", "B", "C", "D"};

            Assert.Throws<ArgumentNullException>(() => sourceCollection.SortAccordingTo(null));
        }

        [Test]
        public void SortAccordingTo_NullSourceCollection_ReturnsException()
        {
            var templateCollection = new[] {"A", "B", "C", "D"};

            Assert.Throws<ArgumentNullException>(() => TemplateSort.SortAccordingTo(null, templateCollection));
        }

        [Test]
        public void SortAccordingTo_NullKeySelector_ReturnsException()
        {
            var sourceCollection = new[] {"A", "B", "C", "D"};
            var templateCollection = new[] {"A", "B", "C", "D"};

            Assert.Throws<ArgumentNullException>(() => sourceCollection.SortAccordingTo(templateCollection, null));
        }

        [Test]
        public void SortAccordingTo_TemplateCollectionThatContainsAllValuesInSourceCollection_ReturnsCorrectSequence()
        {
            var sourceCollection = new[] {"A", "B", "C", "B", "A", "C", "D", "D"};
            var templateCollection = new[] {"B", "A", "D", "C"};

            var result = sourceCollection.SortAccordingTo(templateCollection);

            var expected = new[] {"B", "B", "A", "A", "D", "D", "C", "C"};

            Assert.AreEqual(expected, result);
        }

        [Test]
        public void SortAccordingTo_TemplateCollectionSomeValuesInSourceCollection_ReturnsCorrectSequence()
        {
            var sourceCollection = new[] {"A", "B", "C", "B", "A", "C", "D", "D"};
            var templateCollection = new[] {"B", "D", "C"};

            var result = sourceCollection.SortAccordingTo(templateCollection);

            // Since "A" does not exist int he template collection they are placed at the end of the sequence
            var expected = new[] {"B", "B", "D", "D", "C", "C", "A", "A"};

            Assert.AreEqual(expected, result);
        }

        [Test]
        public void SortAccordingTo_TemplateCollectionContainsNoValuesInSourceCollection_ReturnsCorrectSequence()
        {
            var sourceCollection = new[] {"A", "B", "C", "B", "A", "C", "D", "D"};
            var templateCollection = new[] {"X", "Y", "Z"};

            var result = sourceCollection.SortAccordingTo(templateCollection);

            var expected = new[] {"A", "A", "B", "B", "C", "C", "D", "D"};

            Assert.AreEqual(expected, result);
        }

        [Test]
        public void SortAccordingTo_TemplateCollectionDoesNotContainAllValuesInSourceCollection_ReturnsCorrectSequence2()
        {
            var sourceCollection = new[] {"X", "A", "B", "C", "Y", "B", "A", "C", "Z", "D", "D"};
            var templateCollection = new[] {"B", "D", "C"};

            var result = sourceCollection.SortAccordingTo(templateCollection);

            // Slightly more complicated than the previous test
            // NOTE: no matches get sorted amongst themselves in the order they appear in the list.
            var expected = new[] {"B", "B", "D", "D", "C", "C", "X", "A", "A", "Y", "Z"};

            Assert.AreEqual(expected, result);
        }

        [Test]
        public void SortAccordingTo_TemplateCollectionDoesNotContainAllValuesInSourceCollection_DoNotIncludeNoMatches_ReturnsCorrectSequence()
        {
            var sourceCollection = new[] {"A", "B", "C", "B", "A", "C", "D", "D"};
            var templateCollection = new[] {"B", "D", "C"};

            var result = sourceCollection.SortAccordingTo(templateCollection, false);

            // Since "A" does not exist in the template collection they are placed at the end of the sequence
            var expected = new[] {"B", "B", "D", "D", "C", "C"};

            Assert.AreEqual(expected, result);
        }

        [Test]
        public void SortAccordingTo_TemplateCollectionContainsMoreValuesThanSourceCollection_ReturnsCorrectSequence()
        {
            var sourceCollection = new[] {"A", "B", "C", "B", "A", "C", "D", "D"};
            var templateCollection = new[] {"B", "A", "M", "P", "D", "C"};

            var result = sourceCollection.SortAccordingTo(templateCollection);

            var expected = new[] {"B", "B", "A", "A", "D", "D", "C", "C"};

            Assert.AreEqual(expected, result);
        }

        [Test]
        public void SortAccordingTo_UseComparer_ReturnsCorrectSequence()
        {
            var sourceCollection = new[] {"A", "b", "c", "C", "B", "a", "D", "E", "a", "B"};
            var templateCollection = new[] {"B", "c", "A"};

            var result = sourceCollection.SortAccordingTo(templateCollection, comparer: StringComparer.CurrentCultureIgnoreCase);

            var expected = new[] {"b", "B", "B", "c", "C", "A", "a", "a", "D", "E"};

            Assert.AreEqual(expected, result);
        }

        [Test]
        public void SortAccordingTo_UsingIntegers_ReturnsCorrectSequence()
        {
            var sourceCollection = new[] {2, 3, 9, 4, 1, 6};
            var templateCollection = Enumerable.Range(0, 10);

            var result = sourceCollection.SortAccordingTo(templateCollection);

            var expected = new[] {1, 2, 3, 4, 6, 9};

            Assert.AreEqual(expected, result);
        }

        [Test]
        public void SortAccordingTo_UsingLambdaInteger_ReturnsCorrectSequence()
        {
            var sourceCollection = Helper.GetMyClassCollection();
            var templateCollection = new[] {1, 2, 3, 4};

            var result = sourceCollection.SortAccordingTo(templateCollection, x => x.IntProperty);

            var expected = new[] {"1A", "1A", "2B", "2B", "3C"};

            Assert.AreEqual(expected, result.Select(x => x.Name));
        }

        [Test]
        public void SortAccordingTo_UsingLambdaString_ReturnsCorrectSequence()
        {
            var sourceCollection = Helper.GetMyClassCollection();
            var templateCollection = new[] {"C", "A", "B"};

            var result = sourceCollection.SortAccordingTo(templateCollection, x => x.StringProperty);

            var expected = new[] {"3C", "1A", "1A", "2B", "2B"};

            Assert.AreEqual(expected, result.Select(x => x.Name));
        }
        #endregion
    }

    public class MyClass
    {
        #region Properties
        public string Name { get; set; }
        public string StringProperty { get; set; }
        public int IntProperty { get; set; }
        #endregion
    }

    public static class Helper
    {
        #region Methods
        public static IEnumerable<MyClass> GetMyClassCollection()
        {
            var collection = new List<MyClass>()
            {
                new MyClass()
                {
                    Name = "2B",
                    IntProperty = 2,
                    StringProperty = "B"
                },
                new MyClass()
                {
                    Name = "1A",
                    IntProperty = 1,
                    StringProperty = "A"
                },
                new MyClass()
                {
                    Name = "2B",
                    IntProperty = 2,
                    StringProperty = "B"
                },
                new MyClass()
                {
                    Name = "3C",
                    IntProperty = 3,
                    StringProperty = "C"
                },
                new MyClass()
                {
                    Name = "1A",
                    IntProperty = 1,
                    StringProperty = "A"
                },
            };

            return collection;
        }
        #endregion
    }
}