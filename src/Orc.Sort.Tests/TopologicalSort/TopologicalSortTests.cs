namespace Orc.Sort.Tests.TopologicalSort
{
    using System;
    using System.Collections.Generic;
    using System.Linq;

    using NUnit.Framework;

    using Orc.Sort.TopologicalSort;

    [TestFixture(false)]
    [TestFixture(true)]
    public class TopologicalSortTests
    {
        public TopologicalSortTests(bool usesPriority)
        {
            UsesPriority = usesPriority;
        }

        public bool UsesPriority { get; protected set; }

        [Test]
        public void CanSort_FirstSequenceToBeIncluded_ReturnsTrue()
        {
            var sort = CreateSort<string>();

            var seq_11 = new List<string> { "A", "B", "C", };

            bool result = sort.CanSort(seq_11);

            Assert.True(result);
        }

        [Test]
        public void CanSort_EmptySequence_ReturnsTrue()
        {
            var sort = CreateSort<string>();

            var seq_11 = new List<string> { "A", "B", "C", };
            var seq_21 = new List<string>();

            sort.Add(seq_11);

            bool result = sort.CanSort(seq_21);

            Assert.True(result);
        }

        [Test]
        public void CanSort_NullSequence_ReturnsFalse()
        {
            var sort = CreateSort<string>();

            var seq_11 = new List<string> { "A", "B", "C", };

            sort.Add(seq_11);

            bool result = sort.CanSort(null);

            Assert.False(result);
        }

        [Test]
        public void CanSort_LoopInSequenceIsNOTWeaveable_ReturnsFalse()
        {
            var sort = CreateSort<string>();

            var seq_11 = new List<string> { "A", "B", "C", "A", };

            bool result = sort.CanSort(seq_11);

            Assert.False(result);
        }

        [Test]
        public void CanSort_SecondSequenceIsWeaveable_ReturnsTrue()
        {
            var sort = CreateSort<string>();

            var seq_11 = new List<string> { "A", "B", "C", };
            var seq_12 = new List<string> { "A", "D",      };

            sort.Add(seq_11);

            bool result = sort.CanSort(seq_12);

            Assert.True(result);
        }

        [Test]
        public void CanSort_ThirdSequenceIsNOTWeaveable_ReturnsFalse()
        {
            var sort = CreateSort<string>();

            var seq_11 = new List<string> { "A", "B",      };
            var seq_12 = new List<string> { "A", "C", "D", };
            var seq_13 = new List<string> { "D", "A",      };

            sort.Add(seq_11);
            sort.Add(seq_12);

            bool result = sort.CanSort(seq_13);

            Assert.False(result);
        }

        [Test]
        public void CanSort_SecondSequenceIsNOTWeaveable_ReturnsFalse()
        {
            var sort = CreateSort<string>();

            var seq_11 = new List<string> { "A", "B", "C", };
            var seq_12 = new List<string> { "C", "A",      };

            sort.Add(seq_11);

            bool result = sort.CanSort(seq_12);

            Assert.False(result);
        }

        [Test]
        public void CanSort_IndirectLoop_ReturnsFalse_RdW()
        {
            var sort = CreateSort<string>();

            var seq_11 = new List<string> { "A", "B" };
            var seq_12 = new List<string> { "C", "A" };
            var seq_13 = new List<string> { "D", "B", "C" };

            sort.Add(seq_11);
            sort.Add(seq_12);

            bool result = sort.CanSort(seq_13);

            // Due to seq_11 and seq_12, C is before B (C <- A <- B).
            // Therefore, seq_13 introduces a conflict (C -> B).

            Assert.False(result);
        }

        [Test]
        public void Sort_CollectionOfSequences_ReturnsCorrectSequence()
        {
            var sort = CreateSort<string>();

            var sorted = new List<string> { "A", "B", "C", "D", "E", };

            var seq_11 = new List<string> { "A", "B", "C",           };
            var seq_12 = new List<string> {      "B",      "D", "E", };

            sort.Add(seq_11);
            sort.Add(seq_12);

            var result = sort.Sort();

            AssertOrdering(sorted, result, sort.Sequences);
        }

        [Test]
        public void Sort_CollectionOfSequences_ReturnsCorrectSequence2()
        {
            var sort = CreateSort<string>();

            var sorted = new List<string> { "A", "E", "B", "C", "D", "F", "G", "I", "H", "J", "K", "L", };

            var seq_11 = new List<string> { "A",                                                        };
            var seq_21 = new List<string> {           "B", "C", "D",                                    };
            var seq_22 = new List<string> {      "E", "B",           "F", "G",                          };
            var seq_23 = new List<string> {                "C",           "G",      "H",                };
            var seq_24 = new List<string> {           "B",                     "I", "H",                };
            var seq_31 = new List<string> {                                              "J", "K", "L", };

            sort.Add(seq_11);
            sort.Add(seq_21);
            sort.Add(seq_22);
            sort.Add(seq_23);
            sort.Add(seq_24);
            sort.Add(seq_31);

            var result = sort.Sort();

            AssertOrdering(sorted, result, sort.Sequences);
        }

        [Test]
        public void Sort_CollectionOfSequences_ReturnsCorrectSequence3()
        {
            var sort = CreateSort<string>();

            var sorted = new List<string> { "A", "E", "B", "C", "D", "F", "G", "L", "H", "I", "J", "K", };

            var seq_11 = new List<string> { "A",                                                        };
            var seq_21 = new List<string> {           "B", "C", "D",                                    };
            var seq_22 = new List<string> {      "E", "B",           "F", "G",                          };
            var seq_23 = new List<string> {                "C",           "G",      "H",                };
            var seq_31 = new List<string> {                                              "I", "J", "K", };
            var seq_24 = new List<string> {           "B",                     "L", "H",                };
            
            sort.Add(seq_11);
            sort.Add(seq_21);
            sort.Add(seq_22);
            sort.Add(seq_23);
            sort.Add(seq_31);
            sort.Add(seq_24);

            var result = sort.Sort();

            AssertOrdering(sorted, result, sort.Sequences);
        }

        [Test]
        public void Sort_CollectionOfSequences_ReturnsCorrectSequence4()
        {
            var sort = CreateSort<string>();

            var sorted = new List<string> { "A", "J", "B", "C", "D", "E", "K", "L", "F", "G", "H", "I", "M", "N", };

            var seq_11 = new List<string> { "A",                                                                  };
            var seq_21 = new List<string> {           "B", "C", "D",                                              };
            var seq_22 = new List<string> {           "B",           "E",           "F",                          };
            var seq_31 = new List<string> {                                              "G", "H", "I",           };
            var seq_23 = new List<string> {      "J", "B",                "K", "L",                     "M",      };
            var seq_24 = new List<string> {                "C",                "L", "F",                     "N", };
           
            sort.Add(seq_11);
            sort.Add(seq_21);
            sort.Add(seq_22);
            sort.Add(seq_31);
            sort.Add(seq_23);
            sort.Add(seq_24);

            var result = sort.Sort();

            AssertOrdering(sorted, result, sort.Sequences);
        }

        [Test]
        public void Sort_CollectionOfSequences_ReturnsCorrectSequence5()
        {
            var sort = CreateSort<string>();

            var sorted = new List<string> { "A", "E", "F", "G", "B", "C", "D", };

            var seq_11 = new List<string> { "A",                "B",           };
            var seq_21 = new List<string> {                          "C", "D", };
            var seq_12 = new List<string> {      "E", "F",                     };
            var seq_13 = new List<string> {           "F", "G",                };
            var seq_14 = new List<string> {                "G", "B",           };

            sort.Add(seq_11);
            sort.Add(seq_21);
            sort.Add(seq_12);
            sort.Add(seq_13);
            sort.Add(seq_14);

            var result = sort.Sort();

            AssertOrdering(sorted, result, sort.Sequences);
        }

        [Test]
        public void Sort_CollectionOfSequences_ReturnsCorrectSequence6()
        {
            var sort = CreateSort<string>();

            var sorted = new List<string> { "A", "C", "E", "F", "G", "B", "D", };

            var seq_11 = new List<string> { "A",                     "B",      };
            var seq_12 = new List<string> {      "C",                     "D", };
            var seq_13 = new List<string> {           "E", "F",                };
            var seq_14 = new List<string> {                "F", "G",           };
            var seq_15 = new List<string> {      "C",                "B",      };
            var seq_16 = new List<string> {                     "G", "B",      };
            
            sort.Add(seq_11);
            sort.Add(seq_12);
            sort.Add(seq_13);
            sort.Add(seq_14);
            sort.Add(seq_15);
            sort.Add(seq_16);

            var result = sort.Sort();
            
            AssertOrdering(sorted, result, sort.Sequences);
        }

        [Test]
        public void Sort_CollectionOfSequences_ReturnsCorrectSequence7()
        {
            var sort = CreateSort<string>();

            var sorted = new List<string> { "C", "D", "E", "A", "B", };

            var seq_11 = new List<string> {                "A",      };
            var seq_21 = new List<string> {                     "B", };
            var seq_12 = new List<string> { "C", "D",      "A",      };
            var seq_13 = new List<string> {           "E", "A",      };

            sort.Add(seq_11);
            sort.Add(seq_21);
            sort.Add(seq_12);
            sort.Add(seq_13);

            var result = sort.Sort();

            AssertOrdering(sorted, result, sort.Sequences);
        }

        [Test]
        public void Sort_CollectionOfSequences_ReturnsCorrectSequence8()
        {
            var sort = CreateSort<string>();

            var sorted = new List<string> { "A", "B", "C", "D", "E", };

            var seq_11 = new List<string> { "A",                     };
            var seq_12 = new List<string> { "A", "B",                };
            var seq_13 = new List<string> { "A", "B", "C",           };
            var seq_14 = new List<string> {      "B", "C", "D", "E", };

            sort.Add(seq_11);
            sort.Add(seq_12);
            sort.Add(seq_13);
            sort.Add(seq_14);

            var result = sort.Sort();

            AssertOrdering(sorted, result, sort.Sequences);
        }

        [Test]
        public void Sort_CollectionOfSequences_ReturnsCorrectSequenceRdW()
        {
            var sort = CreateSort<string>();

            var sorted = new List<string> { "B", "G", "I", "A", "C", "D", "E", "F", "H", };

            var seq_11 = new List<string> {                "A",                          };
            var seq_12 = new List<string> { "B",                "C", "D",                };
            var seq_13 = new List<string> {                               "E", "F",      };
            var seq_14 = new List<string> {      "G",      "A",                     "H", };
            var seq_15 = new List<string> {           "I", "A",                          };
            var seq_16 = new List<string> { "B",           "A",                          };

            sort.Add(seq_11);
            sort.Add(seq_12);
            sort.Add(seq_13);
            sort.Add(seq_14);
            sort.Add(seq_15);
            sort.Add(seq_16);

            var result = sort.Sort();

            AssertOrdering(sorted, result, sort.Sequences);
        }

        [Test]
        public void Sort_CollectionOfSequences_ReturnsCorrectSequenceRdW2()
        {
            var sort = CreateSort<string>();

            var sorted = new List<string> { "B", "C", "A", };

            var seq_11 = new List<string> {           "A", };
            var seq_12 = new List<string> { "B",           };
            var seq_13 = new List<string> {      "C", "A", };
            var seq_14 = new List<string> { "B",      "A", };

            sort.Add(seq_11);
            sort.Add(seq_12);
            sort.Add(seq_13);
            sort.Add(seq_14);

            var result = sort.Sort();

            AssertOrdering(sorted, result, sort.Sequences);
        }

        [Test]
        public void Sort_CollectionOfSequences_ReturnsCorrectSequenceRdW3()
        {
            var sort = CreateSort<string>();

            var sorted = new List<string> { "B", "C", "E", "F", "A", "D", };
            var seq_11 = new List<string> {                     "A",      };
            var seq_12 = new List<string> { "B", "C",                "D", };
            var seq_13 = new List<string> {           "E",      "A",      };
            var seq_14 = new List<string> {      "C",      "F", "A" };

            sort.Add(seq_11);
            sort.Add(seq_12);
            sort.Add(seq_13);
            sort.Add(seq_14);

            var result = sort.Sort();

            AssertOrdering(sorted, result, sort.Sequences);
        }


        [Test]
        public void Sort_CollectionOfSequences_ReturnsCorrectSequence_M1()
        {
            var sort = CreateSort<string>();

            var sorted = new List<string> { "A", "J", "B", "C", "D", "E", "Q", "P", "K", "L", "F", "G", "H", "I", "M", "N", "O", };

            var seq_11 = new List<string> { "A",                                                                                 };
            var seq_21 = new List<string> {           "B", "C", "D",                                                             };
            var seq_22 = new List<string> {           "B",           "E",                     "F",                               };
            var seq_23 = new List<string> {                                                        "G", "H", "I",                };
            var seq_24 = new List<string> {      "J", "B",                          "K", "L",                     "M",           };
            var seq_25 = new List<string> {                "C",                          "L", "F",                     "N",      };
            var seq_31 = new List<string> {                                                                                 "O", };
            var seq_26 = new List<string> {                                    "P", "K",                                         };
            var seq_27 = new List<string> {                               "Q", "P",                                              };

            sort.Add(seq_11);
            sort.Add(seq_21);
            sort.Add(seq_22);
            sort.Add(seq_23);
            sort.Add(seq_24);
            sort.Add(seq_25);
            sort.Add(seq_31);
            sort.Add(seq_26);
            sort.Add(seq_27);

            var result = sort.Sort();

            AssertOrdering(sorted, result, sort.Sequences);
        }
        [Test]
        public void Sort_CollectionOfSequences_ReturnsCorrectSequence_M2()
        {
            var sort = CreateSort<string>();

            var sorted = new List<string> { "A", "J", "B", "C", "D", "E", "P", "S", "R", "Q", "K", "L", "F", "G", "H", "I", "M", "N", "O", };

            var seq_11 = new List<string> { "A",                                                                                           };
            var seq_21 = new List<string> {           "B", "C", "D",                                                                       };
            var seq_22 = new List<string> {           "B",           "E",                               "F",                               };
            var seq_23 = new List<string> {                                                                  "G", "H", "I",                };
            var seq_24 = new List<string> {      "J", "B",                                    "K", "L",                     "M",           };
            var seq_25 = new List<string> {                "C",                                    "L", "F",                     "N",      };
            var seq_31 = new List<string> {                                                                                           "O", };
            var seq_26 = new List<string> {                               "P",           "Q", "K",                                         };
            var seq_27 = new List<string> {                                         "R", "Q",                                              };
            var seq_28 = new List<string> {                                    "S", "R",                                                   };
            
            sort.Add(seq_11);
            sort.Add(seq_21);
            sort.Add(seq_22);
            sort.Add(seq_23);
            sort.Add(seq_24);
            sort.Add(seq_25);
            sort.Add(seq_31);
            sort.Add(seq_26);
            sort.Add(seq_27);
            sort.Add(seq_28);

            var result = sort.Sort();

            AssertOrdering(sorted, result, sort.Sequences);
        }

        public void AssertOrdering<T>(IEnumerable<T> sorted, IEnumerable<T> result, IEnumerable<IEnumerable<T>> seq_uences)
        {
            if (UsesPriority)
            {
                Assert.AreEqual(sorted, result);
            }
            else
            {
                var pairs = seq_uences.SelectMany(s => s.Zip(s.Skip(1), (i1, i2) => new Tuple<T, T>(i1, i2)));
                foreach (var pair in pairs)
                {
                    Assert.True(AreInOrder(result, pair.Item1, pair.Item2));
                }
            }
        }

        public static bool AreInOrder<T>(IEnumerable<T> result, T item1, T item2)
        {
            var result_list = ((result as IList<T>) ?? result.ToList()).AsReadOnly();

            var index1 = result_list.IndexOf(item1);
            var index2 = result_list.IndexOf(item2);

            var error_flag = false;
            var error_item = default(T);
            var param_name = "";

            if (index2 == -1)
            {
                error_item = item2;
                param_name ="item2";
            }
            if (index1 == -1)
            {
                error_item = item1;
                param_name ="item1";
            }
            if (error_flag)
            {
                var items_list = String.Join(", ", result.Select(o => o.ToString()));
                var error_text = String.Format("The item: [{0}] was not found in the collection: [{1}]", error_item, items_list);
                throw new ArgumentException(error_text, param_name);
            }

            return (index1 < index2);
        }

        public TopologicalSort<T> CreateSort<T>() where T : IEquatable<T>
        {
            return new TopologicalSort<T>(UsesPriority);
        }
    }
}
