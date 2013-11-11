namespace Orc.Sort.Tests.TemplateSort
{
    using System.Collections.Generic;

    using NUnit.Framework;

    using Orc.Sort.TemplateSort;

    [TestFixture]
    public class NaturalSortTest
    {
        private const string A = "A";
        private const string B = "B";
        private const string C = "C";
        private const string D = "D";
        private const string E = "E";

        private readonly ICollection<string> collection1 = new List<string> { A, B, C, B, A, C, D, D };
        private readonly ICollection<string> template1 = new List<string> { B, A, D, C };
        private readonly ICollection<string> expected1 = new List<string> { B, B, A, A, D, D, C, C };

        private readonly ICollection<string> collection2 = new List<string> { A, B, C, B, A, D, E, A, B };
        private readonly ICollection<string> template2 = new List<string> { B, C, A };
        private readonly ICollection<string> expected2 = new List<string> { B, B, B, C, A, A, A, D, E };

        private readonly ICollection<int> collection3 = new List<int> { 5, 4, 6, 1, 2, 2, 7, 3 };
        private readonly ICollection<int> template3 = new List<int> { 1, 2, 3, 4, 5, 6, 7 };
        private readonly ICollection<int> expected3 = new List<int> { 1, 2, 2, 3, 4, 5, 6, 7 };

        private ICollection<TestClass> collection4;
        private ICollection<int> template4;
        private ICollection<TestClass> expected4;

        private ICollection<TestClass> collection5;
        private ICollection<int> template5;
        private ICollection<TestClass> expected5;

        private class TestClass
        {
            public int V1 { get; set; }
            public int V2 { get; set; }

            public TestClass(int v1, int v2)
            {
                V1 = v1;
                V2 = v2;
            }

            public bool Equals(TestClass other)
            {
                return V1 == other.V1 && V2 == other.V2;
            }

            public override bool Equals(object other)
            {
                if (!(other is TestClass))
                    return false;
                var o = other as TestClass;
                return V1 == o.V1 && V2 == o.V2;
            }
        }

        [SetUp]
        public void Init()
        {
            collection4 = new List<TestClass>()
                            {
                                new TestClass(1,2),
                                new TestClass(2,1),
                                new TestClass(3,2)
                            };
            template4 = new List<int>() { 1, 2 };
            expected4 = new List<TestClass>()
                        {
                            new TestClass(2,1),
                            new TestClass(1,2),
                            new TestClass(3,2)
                        };
            collection5 = new List<TestClass>()
                            {
                                new TestClass(1,2),
                                new TestClass(2,1),
                                new TestClass(3,3),
                                new TestClass(4,2),
                                new TestClass(5,4)
                            };
            template5 = new List<int>() { 1, 2, 3 };
            expected5 = new List<TestClass>()
                        {
                            new TestClass(2,1),
                            new TestClass(1,2),
                            new TestClass(4,2),
                            new TestClass(3,3),
                            new TestClass(5,4)
                        };
        }

        [Test]
        public void EmptyCollectionTest()
        {
            ICollection<string> c = new List<string>();
            var r = c.SortAccordingTo(template1);
            Assert.IsEmpty(r);
        }

        [Test]
        public void EmptyTemplateCollectionTest()
        {
            var r = collection1.SortAccordingTo(new List<string>());
            Assert.That(r, Is.EquivalentTo(collection1));
        }

        [Test]
        public void EmptyBothCollectionsTest()
        {
            ICollection<string> c = new List<string>();
            var r = c.SortAccordingTo(new List<string>());
            Assert.IsEmpty(r);
        }

        [Test]
        public void FirstExampleTest()
        {
            var r = collection1.SortAccordingTo(template1);

            Assert.That(r, Is.EquivalentTo(expected1));
        }

        [Test]
        public void SecondExampleTest()
        {
            var r = collection2.SortAccordingTo(template2);

            Assert.That(r, Is.EquivalentTo(expected2));
        }

        [Test]
        public void IntsExampleTest()
        {
            var r = collection3.SortAccordingTo(template3);

            Assert.That(r, Is.EquivalentTo(expected3));
        }

        [Test, ExpectedException("System.ArgumentException")]
        public void DuplicatedElementsExceptionTest()
        {
            ICollection<string> t = new List<string>() { A, A, B };
            collection1.SortAccordingTo(t);
        }

        [Test]
        public void ReverseSequenceThousandTest()
        {
            ICollection<int> c = new List<int>();
            ICollection<int> t = new List<int>();
            for (var i = 0; i <= 1000; i++)
            {
                c.Add(1000 - i);
                t.Add(i);
            }
            var r = c.SortAccordingTo(t);

            var e = t.GetEnumerator();
            foreach (var v in r)
            {
                e.MoveNext();
                Assert.IsTrue(v.Equals(e.Current));
            }
        }

        [Test]
        public void ReverseSequenceMillionTest()
        {
            ICollection<int> c = new List<int>();
            ICollection<int> t = new List<int>();
            for (var i = 0; i <= 1000000; i++)
            {
                c.Add(1000000 - i);
                t.Add(i);
            }
            var r = c.SortAccordingTo(t);

            var e = t.GetEnumerator();
            foreach (var v in r)
            {
                e.MoveNext();
                Assert.IsTrue(v.Equals(e.Current));
            }
        }

        [Test]
        public void ReverseSequenceTenMillionTest()
        {
            ICollection<int> c = new List<int>();
            ICollection<int> t = new List<int>();
            for (var i = 0; i <= 10000000; i++)
            {
                c.Add(10000000 - i);
                t.Add(i);
            }
            var r = c.SortAccordingTo(t);

            var e = t.GetEnumerator();
            foreach (var v in r)
            {
                e.MoveNext();
                Assert.IsTrue(v.Equals(e.Current));
            }
        }

        [Test]
        public void LambdaEmptyCollectionTest()
        {
            var list = new List<TestClass>();
            var r = list.SortAccordingTo(x => x.V2, template4);

            Assert.IsEmpty(r);
        }

        [Test]
        public void LambdaEmptyTemplateCollectionTest()
        {
            var list = new List<int>();
            var r = collection4.SortAccordingTo(x => x.V2, list);

            Assert.That(r, Is.EquivalentTo(collection4));
        }

        [Test]
        public void LambdaEmptyBothCollectionsTest()
        {
            var list = new List<TestClass>();
            var r = list.SortAccordingTo(x => x.V2, new List<int>());

            Assert.IsEmpty(r);
        }

        [Test]
        public void LambdaExample1Test()
        {
            var r = collection4.SortAccordingTo(x => x.V2, template4);

            AssertCollectionsAreEquals(r, expected4);
        }

        [Test]
        public void LambdaExample2Test()
        {
            var r = collection5.SortAccordingTo(x => x.V2, template5);

            AssertCollectionsAreEquals(r, expected5);
        }

        [Test, ExpectedException("System.ArgumentException")]
        public void LambdaDuplicatedElementsExceptionTest()
        {
            var t = new List<int>() { 1, 2, 2 };
            collection4.SortAccordingTo(x => x.V2, t);
        }


        private void AssertCollectionsAreEquals<T>(ICollection<T> c1, ICollection<T> c2)
        {
            var e = c2.GetEnumerator();
            foreach (var v in c1)
            {
                e.MoveNext();
                Assert.IsTrue(v.Equals(e.Current));
            }
        }
    }
}