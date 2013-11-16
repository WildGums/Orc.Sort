namespace Orc.Sort.Tests.TemplateSort
{
    using System;
    using System.Collections.Generic;
    using System.Linq;
    using Orc.Sort.TemplateSort;

    using NUnit.Framework;

    [TestFixture]
    public class SortingTest
    {
        [Test]
        public void OrderBy()
        {
            string[] sourceCollection = new string[] { "A", "B", "C", "B", "A", "C", "D", "D" };
            IEnumerable<string> result = sourceCollection.OrderBy2(new [] { "B", "A", "D", "C" }, x => x);
            string[] correct = new string[] { "B", "B", "A", "A", "D", "D", "C", "C" };

            int i = 0;
            foreach (string s in result)
            {
                Assert.AreEqual(correct[i], s);
                i++;
            }
        }

        [Test]
        public void OrderByClass()
        {
            var sourceCollection = new List<Foo>()
            {
                Foo.Create("A", 1), 
                Foo.Create("B", 2), 
                Foo.Create("C", 3), 
                Foo.Create("B", 2),
                Foo.Create("A", 1),
                Foo.Create("C", 3), 
                Foo.Create("D", 4), 
                Foo.Create("D", 4), 
            };

            var result = sourceCollection.OrderBy2(new[] { "B", "A", "D", "C" }, x => x.Bar);

            var correct = new[] { "B", "B", "A", "A", "D", "D", "C", "C" };

            Assert.AreEqual(correct, result.Select(x => x.Bar));
        }

        [Test]
        public void OrderByNotMatching()
        {
            string[] sourceCollection = new string[] { "A", "B", "C", "B", "A", "D", "E", "A", "B" };
            IEnumerable<string> result = sourceCollection.OrderBy2(new string[] { "B", "C", "A" }, x => x);
            string[] correct = new string[] { "B", "B", "B", "C", "A", "A", "A", "D", "E" };

            int i = 0;
            foreach (string s in result)
            {
                Assert.AreEqual(correct[i], s);
                i++;
            }
        }

        [Test]
        public void OrderByProperty()
        {
            StringHolder[] sourceCollection = StringHolder.FromStringArray(new string[] { "A", "B", "C", "B", "A", "C", "D", "D" });
            IEnumerable<StringHolder> result = sourceCollection.OrderBy2(new string[] { "B", "A", "D", "C" }, x => x.String);

            string[] correct = new string[] { "B", "B", "A", "A", "D", "D", "C", "C" };
            int i = 0;
            foreach (StringHolder s in result)
            {
                Assert.AreEqual(correct[i], s.String);
                i++;
            }
        }

        [Test]
        public void OrderByComparer()
        {
            string[] sourceCollection = new string[] { "A", "b", "C", "B", "a", "D", "E", "a", "B" };
            IEnumerable<string> result = sourceCollection.OrderBy2(new string[] { "B", "c", "A" }, x => x, StringComparer.CurrentCultureIgnoreCase);
            string[] correct = new string[] { "b", "B", "B", "C", "A", "a", "a", "D", "E" };

            int i = 0;
            foreach (string s in result)
            {
                Assert.AreEqual(correct[i], s);
                i++;
            }
        }

        [Test]
        public void OrderByEmptyTemplate()
        {
            string[] sourceCollection = new string[] { "A", "B", "C", "B", "A", "C", "D", "D" };
            IEnumerable<string> result = sourceCollection.OrderBy2(new string[0], x => x);
            string[] correct = new string[] { "A", "B", "C", "B", "A", "C", "D", "D" };

            int i = 0;
            foreach (string s in result)
            {
                Assert.AreEqual(correct[i], s);
                i++;
            }
        }

        [Test]
        public void OrderByArgsExceptions()
        {
            string[] sourceCollection = new string[] { "A", "B", "C", "B", "A", "D", "E", "A", "B" };

            Assert.Throws<ArgumentNullException>(() => TemplateSort2.OrderBy2<string, string>(null, new string[] { "B", "C", "A" }, x => x));
            Assert.Throws<ArgumentNullException>(() => TemplateSort2.OrderBy2<string, string>(sourceCollection, new string[] { "B", "C", "A" }, null));
            Assert.Throws<ArgumentNullException>(() => TemplateSort2.OrderBy2<string, string>(sourceCollection, null, x => x));
            Assert.Throws<ArgumentException>(() => TemplateSort2.OrderBy2<string, string>(sourceCollection, new string[] { "A", "C", "A" }, x => x));
        }
    }
}
