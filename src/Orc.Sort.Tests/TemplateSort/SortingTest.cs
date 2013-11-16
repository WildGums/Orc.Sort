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
            IEnumerable<string> result = sourceCollection.OrderBy2(x => x, new [] { "B", "A", "D", "C" });
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

            var result = sourceCollection.OrderBy2(x => x.Bar, new[] { "B", "A", "D", "C" });

            var correct = new[] { "B", "B", "A", "A", "D", "D", "C", "C" };

            Assert.AreEqual(correct, result.Select(x => x.Bar));
        }

        [Test]
        public void OrderByNotMatching()
        {
            string[] sourceCollection = new string[] { "A", "B", "C", "B", "A", "D", "E", "A", "B" };
            IEnumerable<string> result = sourceCollection.OrderBy2(x => x, new string[] { "B", "C", "A" });
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
            IEnumerable<StringHolder> result = sourceCollection.OrderBy2(x => x.String, new string[] { "B", "A", "D", "C" });

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
            IEnumerable<string> result = sourceCollection.OrderBy2(x => x, new string[] { "B", "c", "A" }, StringComparer.CurrentCultureIgnoreCase);
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
            IEnumerable<string> result = sourceCollection.OrderBy2(x => x, new string[0]);
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

            Assert.Throws<ArgumentNullException>(() => TemplateSort2.OrderBy2<string, string>(null, x => x, new string[] { "B", "C", "A" }));
            Assert.Throws<ArgumentNullException>(() => TemplateSort2.OrderBy2<string, string>(sourceCollection, null, new string[] { "B", "C", "A" }));
            Assert.Throws<ArgumentNullException>(() => TemplateSort2.OrderBy2<string, string>(sourceCollection, x => x, null));
            Assert.Throws<ArgumentException>(() => TemplateSort2.OrderBy2<string, string>(sourceCollection, x => x, new string[] { "A", "C", "A" }));
        }
    }
}
