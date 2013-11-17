namespace Orc.Sort.Tests.TopologicalSort
{
    using System;
    using System.Collections.Generic;
    using System.Linq;

    using NUnit.Framework;

    using Orc.Sort.TopologicalSort;

    [TestFixture(false, false)] // This will test the normal Topological sort
    [TestFixture(true, true)] // This will test the tracking Topological sort
    public class TopologicalSortTests
    {
        public TopologicalSortTests(bool usesPriority, bool usesTracking)
        {
            UsesPriority = usesPriority;
            UsesTracking = usesTracking;
        }

        public bool UsesPriority { get; protected set; }
        public bool UsesTracking { get; protected set; }

        [Test]
        public void CanSort_FirstSequenceToBeIncluded_ReturnsTrue()
        {
            var sorter = CreateTopologicalSorter<string>();

            var seq_11 = new List<string> { "A", "B", "C", };

            bool result = sorter.CanSort(seq_11);

            Assert.True(result);
        }

        [Test]
        public void Constructor_AcceptsACollection_ReturnsTrue()
        {
            var seq_11 = new List<string> { "A", "B", "C", };
            var sorter = CreateTopologicalSorter<string>(new List<IEnumerable<string>>(){seq_11});

            var result = sorter.Sort();

            Assert.AreEqual(seq_11, result);
        }

        [Test]
        public void CanSort_EmptySequence_ReturnsTrue()
        {
            var sorter = CreateTopologicalSorter<string>();

            var seq_11 = new List<string> { "A", "B", "C", };
            var seq_21 = new List<string>();

            sorter.Add(seq_11);

            bool result = sorter.CanSort(seq_21);

            Assert.True(result);
        }

        [Test]
        public void CanSort_NullSequence_ReturnsFalse()
        {
            var sorter = CreateTopologicalSorter<string>();

            var seq_11 = new List<string> { "A", "B", "C", };

            sorter.Add(seq_11);

            bool result = sorter.CanSort(null);

            Assert.False(result);
        }

        [Test]
        public void CanSort_LoopInSequenceIsNotWeaveable_ReturnsFalse()
        {
            var sorter = CreateTopologicalSorter<string>();

            var seq_11 = new List<string> { "A", "B", "C", "A", };

            bool result = sorter.CanSort(seq_11);

            Assert.False(result);
        }

        [Test]
        public void CanSort_SecondSequenceIsWeaveable_ReturnsTrue()
        {
            var sorter = CreateTopologicalSorter<string>();

            var seq_11 = new List<string> { "A", "B", "C", };
            var seq_12 = new List<string> { "A", "D",      };

            sorter.Add(seq_11);

            bool result = sorter.CanSort(seq_12);

            Assert.True(result);
        }

        [Test]
        public void CanSort_SecondSequenceIsNotWeaveable_ReturnsFalse()
        {
            var sorter = CreateTopologicalSorter<string>();

            var seq_11 = new List<string> { "A", "B", "C", };
            var seq_12 = new List<string> { "C", "A", };

            sorter.Add(seq_11);

            bool result = sorter.CanSort(seq_12);

            Assert.False(result);
        }

        [Test]
        public void CanSort_ThirdSequenceIsNotWeaveable_ReturnsFalse()
        {
            var sorter = CreateTopologicalSorter<string>();

            var seq_11 = new List<string> { "A", "B",      };
            var seq_12 = new List<string> { "A", "C", "D", };
            var seq_13 = new List<string> { "D", "A",      };

            sorter.Add(seq_11);
            sorter.Add(seq_12);

            bool result = sorter.CanSort(seq_13);

            Assert.False(result);
        }

        [Test]
        public void CanSort_IndirectLoop_ReturnsFalse_RdW()
        {
            var sorter = CreateTopologicalSorter<string>();

            var seq_11 = new List<string> { "A", "B" };
            var seq_12 = new List<string> { "C", "A" };
            var seq_13 = new List<string> { "D", "B", "C" };

            sorter.Add(seq_11);
            sorter.Add(seq_12);

            bool result = sorter.CanSort(seq_13);

            // Due to seq_11 and seq_12, C is before B (C <- A <- B).
            // Therefore, seq_13 introduces a conflict (C -> B).

            Assert.False(result);
        }

        [Test]
        public void Sort_CollectionOfSequences_ReturnsCorrectSequence0_0()
        {
            var sorter = CreateTopologicalSorter<string>();

            var sorted = new List<string> { "F", "E", "D", "C", "A", "B", };

            var seq_11 = new List<string> {                     "A", "B", };
            var seq_12 = new List<string> {                "C", "A",      };
            var seq_13 = new List<string> {           "D", "C",           };
            var seq_14 = new List<string> {           "D", "C",           };
            var seq_15 = new List<string> {      "E", "D",                };
            var seq_16 = new List<string> { "F", "E",                     };

            sorter.Add(seq_11);
            sorter.Add(seq_12);
            sorter.Add(seq_13);
            sorter.Add(seq_14);
            sorter.Add(seq_15);
            sorter.Add(seq_16);

            var result = sorter.Sort();

            AssertOrdering(sorted, result, sorter.Sequences);

            Assert.True("F".ComesBefore(sorted.Skip(1), result));
            Assert.True("B".ComesAfter(sorted.Take(sorted.Count - 1), result));

//          Assert.AreEqual(1, sorter.components.Distinct().Count());
        }

        [Test]
        public void Sort_CollectionOfSequences_ReturnsCorrectSequence0_DefaultOrder()
        {
            var sorter = CreateTopologicalSorter<string>();

            var sorted = new List<string> { "A", "B", "E", "F", "D", "C", };

            var seq_11 = new List<string> { "A", "B",                "C", };
            var seq_12 = new List<string> {      "B",           "D", "C", };
            var seq_13 = new List<string> {      "B", "E",      "D",      };
            var seq_14 = new List<string> {           "E", "F", "D",      };

            sorter.Add(seq_11);
            sorter.Add(seq_12);
            sorter.Add(seq_13);
            sorter.Add(seq_14);

            var result = sorter.Sort();

            AssertOrdering(sorted, result, sorter.Sequences);

            Assert.True("A".ComesBefore(sorted.Skip(1), result));
            Assert.True("C".ComesAfter(sorted.Take(sorted.Count - 1), result));

//          Assert.AreEqual(1, sorter.components.Distinct().Count());
        }

        [Test]
        public void Sort_CollectionOfSequences_ReturnsCorrectSequence0_DifferentOrder()
        {
            var sorter = CreateTopologicalSorter<string>();

            var sorted = new List<string> { "A", "B", "E", "F", "D", "C", };

            var seq_11 = new List<string> { "A", "B",                "C", };
            var seq_12 = new List<string> {      "B",           "D", "C", };
            var seq_13 = new List<string> {      "B", "E",      "D",      };
            var seq_14 = new List<string> {           "E", "F", "D",      };

            sorter.Add(seq_14);
            sorter.Add(seq_12);
            sorter.Add(seq_13);
            sorter.Add(seq_11);

            var result = sorter.Sort();

            // different list order doesn't make a difference in this case,
            // because there's only one possible sort order for this graph.
            
            AssertOrdering(sorted, result, sorter.Sequences);

            Assert.True("A".ComesBefore(sorted.Skip(1), result));
            Assert.True("C".ComesAfter(sorted.Take(sorted.Count - 1), result));

//          Assert.AreEqual(1, sorter.components.Distinct().Count());
        }

        [Test]
        public void Sort_CollectionOfSequences_ReturnsCorrectSequence1()
        {
            var sorter = CreateTopologicalSorter<string>();

            var sorted = new List<string> { "A", "B", "C", "D", "E", };

            var seq_11 = new List<string> { "A", "B", "C",           };
            var seq_12 = new List<string> {      "B",      "D", "E", };

            sorter.Add(seq_11);
            sorter.Add(seq_12);

            var result = sorter.Sort();

            AssertOrdering(sorted, result, sorter.Sequences);
            Assert.True("B".ComesBefore(new List<string>(){"C", "D", "E"}, result));

//          Assert.AreEqual(1, sorter.components.Distinct().Count());
        }

        [Test]
        public void Sort_CollectionOfSequences_ReturnsCorrectSequence2()
        {
            var sorter = CreateTopologicalSorter<string>();

            var sorted = new List<string> { "A", "E", "B", "C", "D", "F", "G", "I", "H", "J", "K", "L", };
            
            var seq_11 = new List<string> { "A",                                                        };
            var seq_21 = new List<string> {           "B", "C", "D",                                    };
            var seq_22 = new List<string> {      "E", "B",           "F", "G",                          };
            var seq_23 = new List<string> {                "C",           "G",      "H",                };
            var seq_24 = new List<string> {           "B",                     "I", "H",                };
            var seq_31 = new List<string> {                                              "J", "K", "L", };

            sorter.Add(seq_11);
            sorter.Add(seq_21);
            sorter.Add(seq_22);
            sorter.Add(seq_23);
            sorter.Add(seq_24);
            sorter.Add(seq_31);

            var result = sorter.Sort();

            AssertOrdering(sorted, result, sorter.Sequences);

//          Assert.AreEqual(3, sorter.components.Distinct().Count());
        }

        [Test]
        public void Sort_CollectionOfSequences_ReturnsCorrectSequence3()
        {
            var sorter = CreateTopologicalSorter<string>();

            var sorted = new List<string> { "A", "E", "B", "C", "D", "F", "G", "L", "H", "I", "J", "K", };

            var seq_11 = new List<string> { "A",                                                        };
            var seq_21 = new List<string> {           "B", "C", "D",                                    };
            var seq_22 = new List<string> {      "E", "B",           "F", "G",                          };
            var seq_23 = new List<string> {                "C",           "G",      "H",                };
            var seq_31 = new List<string> {                                              "I", "J", "K", };
            var seq_24 = new List<string> {           "B",                     "L", "H",                };
            
            sorter.Add(seq_11);
            sorter.Add(seq_21);
            sorter.Add(seq_22);
            sorter.Add(seq_23);
            sorter.Add(seq_31);
            sorter.Add(seq_24);

            var result = sorter.Sort();

            AssertOrdering(sorted, result, sorter.Sequences);

//          Assert.AreEqual(3, sorter.components.Distinct().Count());
        }

        [Test]
        public void Sort_CollectionOfSequences_ReturnsCorrectSequence4()
        {
            var sorter = CreateTopologicalSorter<string>();

            var sorted = new List<string> { "A", "J", "B", "C", "D", "E", "K", "L", "F", "G", "H", "I", "M", "N", };

            var seq_11 = new List<string> { "A",                                                                  };
            var seq_21 = new List<string> {           "B", "C", "D",                                              };
            var seq_22 = new List<string> {           "B",           "E",           "F",                          };
            var seq_31 = new List<string> {                                              "G", "H", "I",           };
            var seq_23 = new List<string> {      "J", "B",                "K", "L",                     "M",      };
            var seq_24 = new List<string> {                "C",                "L", "F",                     "N", };
           
            sorter.Add(seq_11);
            sorter.Add(seq_21);
            sorter.Add(seq_22);
            sorter.Add(seq_31);
            sorter.Add(seq_23);
            sorter.Add(seq_24);

            var result = sorter.Sort();

            AssertOrdering(sorted, result, sorter.Sequences);

//          Assert.AreEqual(3, sorter.components.Distinct().Count());
        }

        [Test]
        public void Sort_CollectionOfSequences_ReturnsCorrectSequence5()
        {
            var sorter = CreateTopologicalSorter<string>();

            var sorted = new List<string> { "A", "E", "F", "G", "B", "C", "D", };

            var seq_11 = new List<string> { "A",                "B",           };
            var seq_21 = new List<string> {                          "C", "D", };
            var seq_12 = new List<string> {      "E", "F",                     };
            var seq_13 = new List<string> {           "F", "G",                };
            var seq_14 = new List<string> {                "G", "B",           };

            sorter.Add(seq_11);
            sorter.Add(seq_21);
            sorter.Add(seq_12);
            sorter.Add(seq_13);
            sorter.Add(seq_14);

            var result = sorter.Sort();

            AssertOrdering(sorted, result, sorter.Sequences);

//          Assert.AreEqual(2, sorter.components.Distinct().Count());
        }

        [Test]
        public void Sort_CollectionOfSequences_ReturnsCorrectSequence6()
        {
            var sorter = CreateTopologicalSorter<string>();

            var sorted = new List<string> { "A", "C", "E", "F", "G", "B", "D", };

            var seq_11 = new List<string> { "A",                     "B",      };
            var seq_12 = new List<string> {      "C",                     "D", };
            var seq_13 = new List<string> {           "E", "F",                };
            var seq_14 = new List<string> {                "F", "G",           };
            var seq_15 = new List<string> {      "C",                "B",      };
            var seq_16 = new List<string> {                     "G", "B",      };
            
            sorter.Add(seq_11);
            sorter.Add(seq_12);
            sorter.Add(seq_13);
            sorter.Add(seq_14);
            sorter.Add(seq_15);
            sorter.Add(seq_16);

            var result = sorter.Sort();
            
            AssertOrdering(sorted, result, sorter.Sequences);

//          Assert.AreEqual(1, sorter.components.Distinct().Count());
        }

        [Test]
        public void Sort_CollectionOfSequences_ReturnsCorrectSequence7()
        {
            var sorter = CreateTopologicalSorter<string>();

            var sorted = new List<string> { "C", "D", "E", "A", "B", };

            var seq_11 = new List<string> {                "A",      };
            var seq_21 = new List<string> {                     "B", };
            var seq_12 = new List<string> { "C", "D",      "A",      };
            var seq_13 = new List<string> {           "E", "A",      };

            sorter.Add(seq_11);
            sorter.Add(seq_21);
            sorter.Add(seq_12);
            sorter.Add(seq_13);

            var result = sorter.Sort();

            AssertOrdering(sorted, result, sorter.Sequences);
            Assert.True("A".ComesAfter(new List<string>(){"C", "D", "E"}, result));

//          Assert.AreEqual(2, sorter.components.Distinct().Count());
        }

        [Test]
        public void Sort_CollectionOfSequences_ReturnsCorrectSequence8()
        {
            var sorter = CreateTopologicalSorter<string>();

            var sorted = new List<string> { "A", "B", "C", "D", "E", };

            var seq_11 = new List<string> { "A",                     };
            var seq_12 = new List<string> { "A", "B",                };
            var seq_13 = new List<string> { "A", "B", "C",           };
            var seq_14 = new List<string> {      "B", "C", "D", "E", };

            sorter.Add(seq_11);
            sorter.Add(seq_12);
            sorter.Add(seq_13);
            sorter.Add(seq_14);

            var result = sorter.Sort();

            AssertOrdering(sorted, result, sorter.Sequences);
            Assert.True("E".ComesAfter(new List<string>() { "A", "B", "C", "D" }, result));

//          Assert.AreEqual(1, sorter.components.Distinct().Count());
        }

        [Test]
        public void Sort_CollectionOfSequences_ReturnsCorrectSequenceRdW()
        {
            var sorter = CreateTopologicalSorter<string>();

            var sorted = new List<string> { "B", "G", "I", "A", "C", "D", "E", "F", "H", };

            var seq_11 = new List<string> {                "A",                          };
            var seq_12 = new List<string> { "B",                "C", "D",                };
            var seq_21 = new List<string> {                               "E", "F",      };
            var seq_13 = new List<string> {      "G",      "A",                     "H", };
            var seq_14 = new List<string> {           "I", "A",                          };
            var seq_15 = new List<string> { "B",           "A",                          };

            sorter.Add(seq_11);
            sorter.Add(seq_12);
            sorter.Add(seq_21);
            sorter.Add(seq_13);
            sorter.Add(seq_14);
            sorter.Add(seq_15);

            var result = sorter.Sort();

            AssertOrdering(sorted, result, sorter.Sequences);

//          Assert.AreEqual(2, sorter.components.Distinct().Count());
        }

        [Test]
        public void Sort_CollectionOfSequences_ReturnsCorrectSequenceRdW2()
        {
            var sorter = CreateTopologicalSorter<string>();

            var sorted = new List<string> { "B", "C", "A", };

            var seq_11 = new List<string> {           "A", };
            var seq_12 = new List<string> { "B",           };
            var seq_13 = new List<string> {      "C", "A", };
            var seq_14 = new List<string> { "B",      "A", };

            sorter.Add(seq_11);
            sorter.Add(seq_12);
            sorter.Add(seq_13);
            sorter.Add(seq_14);

            var result = sorter.Sort();

            AssertOrdering(sorted, result, sorter.Sequences);

//          Assert.AreEqual(1, sorter.components.Distinct().Count());
        }

        [Test]
        public void Sort_CollectionOfSequences_ReturnsCorrectSequenceRdW3()
        {
            var sorter = CreateTopologicalSorter<string>();

            var sorted = new List<string> { "B", "C", "E", "F", "A", "D", };
            var seq_11 = new List<string> {                     "A",      };
            var seq_12 = new List<string> { "B", "C",                "D", };
            var seq_13 = new List<string> {           "E",      "A",      };
            var seq_14 = new List<string> {      "C",      "F", "A" };

            sorter.Add(seq_11);
            sorter.Add(seq_12);
            sorter.Add(seq_13);
            sorter.Add(seq_14);

            var result = sorter.Sort();

            AssertOrdering(sorted, result, sorter.Sequences);

//          Assert.AreEqual(1, sorter.components.Distinct().Count());
        }

        [Test]
        public void Sort_CollectionOfSequences_ReturnsCorrectSequence_M1()
        {
            var sorter = CreateTopologicalSorter<string>();

            var sorted = new List<string> { "A", "J", "B", "C", "D", "E", "Q", "P", "K", "L", "F", "G", "H", "I", "M", "N", "O", };

            var seq_11 = new List<string> { "A",                                                                                 };
            var seq_21 = new List<string> {           "B", "C", "D",                                                             };
            var seq_22 = new List<string> {           "B",           "E",                     "F",                               };
            var seq_31 = new List<string> {                                                        "G", "H", "I",                };
            var seq_23 = new List<string> {      "J", "B",                          "K", "L",                     "M",           };
            var seq_24 = new List<string> {                "C",                          "L", "F",                     "N",      };
            var seq_41 = new List<string> {                                                                                 "O", };
            var seq_25 = new List<string> {                                    "P", "K",                                         };
            var seq_26 = new List<string> {                               "Q", "P",                                              };

            sorter.Add(seq_11);
            sorter.Add(seq_21);
            sorter.Add(seq_22);
            sorter.Add(seq_31);
            sorter.Add(seq_23);
            sorter.Add(seq_24);
            sorter.Add(seq_41);
            sorter.Add(seq_25);
            sorter.Add(seq_26);

            var result = sorter.Sort();

            AssertOrdering(sorted, result, sorter.Sequences);

//          Assert.AreEqual(4, sorter.components.Distinct().Count());
        }

        [Test]
        public void Sort_CollectionOfSequences_ReturnsCorrectSequence_M2()
        {
            var sorter = CreateTopologicalSorter<string>();

            var sorted = new List<string> { "A", "J", "B", "C", "D", "E", "P", "S", "R", "Q", "K", "L", "F", "G", "H", "I", "M", "N", "O", };

            var seq_11 = new List<string> { "A",                                                                                           };
            var seq_21 = new List<string> {           "B", "C", "D",                                                                       };
            var seq_22 = new List<string> {           "B",           "E",                               "F",                               };
            var seq_31 = new List<string> {                                                                  "G", "H", "I",                };
            var seq_23 = new List<string> {      "J", "B",                                    "K", "L",                     "M",           };
            var seq_24 = new List<string> {                "C",                                    "L", "F",                     "N",      };
            var seq_41 = new List<string> {                                                                                           "O", };
            var seq_25 = new List<string> {                               "P",           "Q", "K",                                         };
            var seq_26 = new List<string> {                                         "R", "Q",                                              };
            var seq_27 = new List<string> {                                    "S", "R",                                                   };
            
            sorter.Add(seq_11);
            sorter.Add(seq_21);
            sorter.Add(seq_22);
            sorter.Add(seq_31);
            sorter.Add(seq_23);
            sorter.Add(seq_24);
            sorter.Add(seq_41);
            sorter.Add(seq_25);
            sorter.Add(seq_26);
            sorter.Add(seq_27);

            var result = sorter.Sort();

            AssertOrdering(sorted, result, sorter.Sequences);

//          Assert.AreEqual(4, sorter.components.Distinct().Count());
        }

        [Test]
        public void GetConflicts_CollectionOfSequences_CanNotBeSorted1()
        {
            var sorter = CreateTopologicalSorter<string>();

            var seq_11 = new List<string>() { "A", "B", "C", "D" };
            var seq_21 = new List<string>() { "B", "A", "C", };

            sorter.Add(seq_11);
            sorter.Add(seq_21);

            var result = sorter.GetConflicts();

            Assert.AreEqual(seq_11, result.First());
            Assert.AreEqual(seq_21, result.Last());
        }

        [Test]
        public void GetConflicts_CollectionOfSequences_CanNotBeSorted2()
        {
            var sorter = CreateTopologicalSorter<string>();

            var seq_11 = new List<string>() { "A", "B", "C", "D" };
            var seq_12 = new List<string>() { "A", "B", };
            var seq_21 = new List<string>() { "B", "A", "C", };

            sorter.Add(seq_11);
            sorter.Add(seq_12);
            sorter.Add(seq_21);

            var result = sorter.GetConflicts();

            Assert.AreEqual(seq_11, result.First());
            Assert.AreEqual(seq_21, result.Last());
        }

        [Test]
        public void GetConflicts_CollectionOfSequences_CanNotBeSorted3()
        {
            var sorter = CreateTopologicalSorter<string>();

            var seq_01 = new List<string>() { "E", "F" };
            var seq_11 = new List<string>() { "A", "B", "C", "D" };
            var seq_12 = new List<string>() { "C", "D", };
            var seq_21 = new List<string>() { "B", "A", "C", };

            sorter.Add(seq_01);
            sorter.Add(seq_11);
            sorter.Add(seq_12);
            sorter.Add(seq_21);

            var result = sorter.GetConflicts();

            Assert.AreEqual(seq_11, result.First());
            Assert.AreEqual(seq_21, result.Last());
        }

        [Test]
        public void GetConflicts_CollectionOfSequences_CanNotBeSorted4()
        {
            var sorter = CreateTopologicalSorter<string>();

            var seq_01 = new List<string>() { "E", "F" };
            var seq_11 = new List<string>() { "A", "B", "C", "D" };
            var seq_12 = new List<string>() { "A", "B", };
            var seq_21 = new List<string>() { "B", "A", "C", };
            var seq_31 = new List<string>() { "M", "N", "O", };

            sorter.Add(seq_01);
            sorter.Add(seq_11);
            sorter.Add(seq_12);
            sorter.Add(seq_21);
            sorter.Add(seq_31);

            var result = sorter.GetConflicts();

            Assert.AreEqual(seq_11, result.First());
            Assert.AreEqual(seq_21, result.Last());
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

        public TopologicalSort<T> CreateTopologicalSorter<T>(IEnumerable<IEnumerable<T>> collection = null)
        {
            return new TopologicalSort<T>(UsesPriority, UsesTracking, collection);
        }
    }
}
