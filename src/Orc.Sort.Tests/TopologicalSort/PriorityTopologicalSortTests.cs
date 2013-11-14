namespace Orc.Sort.Tests.TopologicalSort
{
    using System.Collections.Generic;

    using NUnit.Framework;

    using Orc.Sort.TopologicalSort;

    [TestFixture]
    public class PriorityTopologicalSortTests
    {
        [Test]
        public void CanWeave_FirstListToBeIncluded_ReturnsTrue()
        {
            // Arrange
            var lw = new PriorityTopologicalSort<string>();
            var list1 = new List<string> { "A", "B", "C", };

            // Act
            bool result = lw.CanSort(list1);

            // Assert
            Assert.True(result);
        }

        [Test]
        public void CanWeave_EmptyList_ReturnsTrue()
        {
            // Arrange
            var lw = new PriorityTopologicalSort<string>();
            var list1 = new List<string> { "A", "B", "C", };
            var list2 = new List<string>();

            // Act
            lw.Add(list1);
            bool result = lw.CanSort(list2);

            // Assert
            Assert.True(result);
        }

        [Test]
        public void CanWeave_NullList_ReturnsFalse()
        {
            // Arrange
            var lw = new PriorityTopologicalSort<string>();
            var list1 = new List<string> { "A", "B", "C", };

            // Act
            lw.Add(list1);
            bool result = lw.CanSort(null);

            // Assert
            Assert.False(result);
        }

        [Test]
        public void CanWeave_LoopInListIsNOTWeaveable_ReturnsFalse()
        {
            // Arrange
            var lw = new PriorityTopologicalSort<string>();
            var list1 = new List<string> { "A", "B", "C", "A", };

            // Act
            bool result = lw.CanSort(list1);

            // Assert
            Assert.False(result);
        }

        [Test]
        public void CanWeave_SecondListIsWeaveable_ReturnsTrue()
        {
            // Arrange
            var lw = new PriorityTopologicalSort<string>();
            var list1 = new List<string> { "A", "B", "C", };
            var list2 = new List<string> { "A", "D",      };

            // Act
            lw.Add(list1);
            bool result = lw.CanSort(list2);

            // Assert
            Assert.True(result);
        }

        [Test]
        public void CanWeave_ThirdListIsNOTWeaveable_ReturnsFalse()
        {
            // Arrange
            var lw = new PriorityTopologicalSort<string>();
            var list1 = new List<string> { "A", "B",      };
            var list2 = new List<string> { "A", "C", "D", };
            var list3 = new List<string> { "D", "A",      };

            // Act
            lw.Add(list1);
            lw.Add(list2);
            bool result = lw.CanSort(list3);

            // Assert
            Assert.False(result);
        }

        [Test]
        public void CanWeave_SecondListIsNOTWeaveable_ReturnsFalse()
        {
            // Arrange
            var lw = new PriorityTopologicalSort<string>();
            var list1 = new List<string> { "A", "B", "C", };
            var list2 = new List<string> { "C", "A",      };

            // Act
            lw.Add(list1);
            bool result = lw.CanSort(list2);

            // Assert
            Assert.False(result);
        }

        [Test]
        public void CanWeave_CollectionOfLists_ReturnsCorrectSequence()
        {
            // Arrange
            var lw = new PriorityTopologicalSort<string>();

            var sort0 = new List<string> { "A", "B", "C", "D", "E", };
            var list1 = new List<string> { "A", "B", "C",           };
            var list2 = new List<string> {      "B",      "D", "E", };

            // Act
            lw.Add(list1);
            lw.Add(list2);
            var result = lw.Sort();

            // Assert
            Assert.AreEqual(sort0, result );
        }

        [Test]
        public void CanWeave_IndirectLoop_ReturnsFalse_RdW()
        {
            // Arrange
            var lw = new PriorityTopologicalSort<string>();
            var list1 = new List<string> { "A", "B" };
            var list2 = new List<string> { "C", "A" };
            var list3 = new List<string> { "D", "B", "C" };

            // Act
            lw.Add(list1);
            lw.Add(list2);
            bool result = lw.CanSort(list3);

            // Due to list1 and list2, C is before B (C <- A <- B).
            // list3 therefore introduces a conflict (C -> B).

            // Assert
            Assert.False(result);
        }

        [Test]
        public void CanWeave_CollectionOfLists_ReturnsCorrectSequence2()
        {
            // Arrange
            var lw = new PriorityTopologicalSort<string>();

            var sorted = new List<string> { "A", "E", "B", "C", "D", "F", "G", "I", "H", "J", "K", "L", };

            var list11 = new List<string> { "A",                                                        };
            var list21 = new List<string> {           "B", "C", "D",                                    };
            var list22 = new List<string> {      "E", "B",           "F", "G",                          };
            var list23 = new List<string> {                "C",           "G",      "H",                };
            var list24 = new List<string> {           "B",                     "I", "H",                };
            var list31 = new List<string> {                                              "J", "K", "L", };

            // Act
            lw.Add(list11);
            lw.Add(list21);
            lw.Add(list22);
            lw.Add(list23);
            lw.Add(list24);
            lw.Add(list31);
            var result = lw.Sort();

            // Assert
            Assert.AreEqual(sorted, result);
        }

        [Test]
        public void CanWeave_CollectionOfLists_ReturnsCorrectSequence3()
        {
            // Arrange
            var lw = new PriorityTopologicalSort<string>();

            var sort0 = new List<string> { "A", "E", "B", "C", "D", "F", "G", "L", "H", "I", "J", "K", };

            var list0 = new List<string> { "A",                                                        };
            var list1 = new List<string> {           "B", "C", "D",                                    };
            var list2 = new List<string> {      "E", "B",           "F", "G",                          };
            var list3 = new List<string> {                "C",           "G",      "H",                };
            var list4 = new List<string> {                                              "I", "J", "K", };
            var list5 = new List<string> {           "B",                     "L", "H",                };
            
            // Act
            lw.Add(list0);
            lw.Add(list1);
            lw.Add(list2);
            lw.Add(list3);
            lw.Add(list4);
            lw.Add(list5);
            var result = lw.Sort();

            // Assert
            Assert.AreEqual(sort0, result);
        }

        [Test]
        public void CanWeave_CollectionOfLists_ReturnsCorrectSequence4()
        {
            // Arrange
            var lw = new PriorityTopologicalSort<string>();

            var sorted = new List<string> { "A", "J", "B", "C", "D", "E", "K", "L", "F", "G", "H", "I", "M", "N", };

            var list11 = new List<string> { "A",                                                                  };
            var list21 = new List<string> {           "B", "C", "D",                                              };
            var list22 = new List<string> {           "B",           "E",           "F",                          };
            var list31 = new List<string> {                                              "G", "H", "I",           };
            var list23 = new List<string> {      "J", "B",                "K", "L",                     "M",      };
            var list24 = new List<string> {                "C",                "L", "F",                     "N", };
           
            // Act
            lw.Add(list11);
            lw.Add(list21);
            lw.Add(list22);
            lw.Add(list31);
            lw.Add(list23);
            lw.Add(list24);
            var result = lw.Sort();

            // Assert
            Assert.AreEqual(sorted, result);
        }

        [Test]
        public void CanWeave_CollectionOfLists_ReturnsCorrectSequence5()
        {
            // Arrange
            var lw = new PriorityTopologicalSort<string>();

            var sorted = new List<string> { "A", "E", "F", "G", "B", "C", "D", };

            var list11 = new List<string> { "A",                "B",           };
            var list21 = new List<string> {                          "C", "D", };
            var list12 = new List<string> {      "E", "F",                     };
            var list13 = new List<string> {           "F", "G",                };
            var list14 = new List<string> {                "G", "B",           };

            // Act
            lw.Add(list11);
            lw.Add(list21);
            lw.Add(list12);
            lw.Add(list13);
            lw.Add(list14);
            var result = lw.Sort();

            // Assert
            Assert.AreEqual(sorted, result);
        }

        [Test]
        public void CanWeave_CollectionOfLists_ReturnsCorrectSequence6()
        {
            // Arrange
            var lw = new PriorityTopologicalSort<string>();

            var sorted = new List<string> { "A", "C", "E", "F", "G", "B", "D", };

            var list11 = new List<string> { "A",                     "B",      };
            var list12 = new List<string> {      "C",                     "D", };
            var list13 = new List<string> {           "E", "F",                };
            var list14 = new List<string> {                "F", "G",           };
            var list15 = new List<string> {      "C",                "B",      };
            var list16 = new List<string> {                     "G", "B",      };
            
            // Act
            lw.Add(list11);
            lw.Add(list12);
            lw.Add(list13);
            lw.Add(list14);
            lw.Add(list15);
            lw.Add(list16);
            var result = lw.Sort();
            
            // Assert
            Assert.AreEqual(sorted, result);
        }

        [Test]
        public void CanWeave_CollectionOfLists_ReturnsCorrectSequence7()
        {
            // Arrange
            var lw = new PriorityTopologicalSort<string>();

            var sorted = new List<string> { "C", "D", "E", "A", "B", };

            var list11 = new List<string> {                "A",      };
            var list21 = new List<string> {                     "B", };
            var list12 = new List<string> { "C", "D",      "A",      };
            var list13 = new List<string> {           "E", "A",      };

            // Act
            lw.Add(list11);
            lw.Add(list21);
            lw.Add(list12);
            lw.Add(list13);
            var result = lw.Sort();

            // Assert
            Assert.AreEqual(sorted, result);
        }

        [Test]
        public void CanWeave_CollectionOfLists_ReturnsCorrectSequence8()
        {
            // Arrange
            var lw = new PriorityTopologicalSort<string>();

            var sorted = new List<string> { "A", "B", "C", "D", "E", };

            var list11 = new List<string> { "A",                     };
            var list12 = new List<string> { "A", "B",                };
            var list13 = new List<string> { "A", "B", "C",           };
            var list14 = new List<string> {      "B", "C", "D", "E", };

            // Act
            lw.Add(list11);
            lw.Add(list12);
            lw.Add(list13);
            lw.Add(list14);
            var result = lw.Sort();

            // Assert
            Assert.AreEqual(sorted, result);
        }

        [Test]
        public void CanWeave_CollectionOfLists_ReturnsCorrectSequenceRdW()
        {
            // Arrange
            var lw = new PriorityTopologicalSort<string>();

            var sorted = new List<string> { "B", "G", "I", "A", "C", "D", "E", "F", "H", };

            var list11 = new List<string> {                "A",                          };
            var list12 = new List<string> { "B",                "C", "D",                };
            var list13 = new List<string> {                               "E", "F",      };
            var list14 = new List<string> {      "G",      "A",                     "H", };
            var list15 = new List<string> {           "I", "A",                          };
            var list16 = new List<string> { "B",           "A",                          };

            // Act
            lw.Add(list11);
            lw.Add(list12);
            lw.Add(list13);
            lw.Add(list14);
            lw.Add(list15);
            lw.Add(list16);
            var result = lw.Sort();

            // Assert
            Assert.AreEqual(sorted, result);
        }

        [Test]
        public void CanWeave_CollectionOfLists_ReturnsCorrectSequenceRdW2()
        {
            // Arrange
            var lw = new PriorityTopologicalSort<string>();

            var sorted = new List<string> { "B", "C", "A", };

            var list11 = new List<string> {           "A", };
            var list12 = new List<string> { "B",           };
            var list13 = new List<string> {      "C", "A", };
            var list14 = new List<string> { "B",      "A", };

            // Act
            lw.Add(list11);
            lw.Add(list12);
            lw.Add(list13);
            lw.Add(list14);
            var result = lw.Sort();

            // Assert
            Assert.AreEqual(sorted, result);
        }

        [Test]
        public void CanWeave_CollectionOfLists_ReturnsCorrectSequenceRdW3()
        {
            // Arrange
            var lw = new PriorityTopologicalSort<string>();

            var sorted = new List<string> { "B", "C", "E", "F", "A", "D", };
            var list11 = new List<string> {                     "A",      };
            var list12 = new List<string> { "B", "C",                "D", };
            var list13 = new List<string> {           "E",      "A",      };
            var list14 = new List<string> {      "C",      "F", "A" };

            // Act
            lw.Add(list11);
            lw.Add(list12);
            lw.Add(list13);
            lw.Add(list14);
            var result = lw.Sort();

            // Assert
            Assert.AreEqual(sorted, result);
        }


        [Test]
        public void CanWeave_CollectionOfLists_ReturnsCorrectSequence_M1()
        {
            // Arrange
            var lw = new PriorityTopologicalSort<string>();

            var sorted = new List<string> { "A", "J", "B", "C", "D", "E", "Q", "P", "K", "L", "F", "G", "H", "I", "M", "N", "O", };

            var list11 = new List<string> { "A",                                                                                 };
            var list21 = new List<string> {           "B", "C", "D",                                                             };
            var list22 = new List<string> {           "B",           "E",                     "F",                               };
            var list23 = new List<string> {                                                        "G", "H", "I",                };
            var list24 = new List<string> {      "J", "B",                          "K", "L",                     "M",           };
            var list25 = new List<string> {                "C",                          "L", "F",                     "N",      };
            var list31 = new List<string> {                                                                                 "O", };
            var list26 = new List<string> {                                    "P", "K",                                         };
            var list27 = new List<string> {                               "Q", "P",                                              };

            lw.Add(list11);
            lw.Add(list21);
            lw.Add(list22);
            lw.Add(list23);
            lw.Add(list24);
            lw.Add(list25);
            lw.Add(list31);
            lw.Add(list26);
            lw.Add(list27);
            var result = lw.Sort();

            // Assert
            Assert.AreEqual(sorted, result);
        }
        [Test]
        public void CanWeave_CollectionOfLists_ReturnsCorrectSequence_M2()
        {
            // Arrange
            var lw = new PriorityTopologicalSort<string>();


            
            var sorted = new List<string> { "A", "J", "B", "C", "D", "E", "P", "S", "R", "Q", "K", "L", "F", "G", "H", "I", "M", "N", "O", };

            var list11 = new List<string> { "A",                                                                                           };
            var list21 = new List<string> {           "B", "C", "D",                                                                       };
            var list22 = new List<string> {           "B",           "E",                               "F",                               };
            var list23 = new List<string> {                                                                  "G", "H", "I",                };
            var list24 = new List<string> {      "J", "B",                                    "K", "L",                     "M",           };
            var list25 = new List<string> {                "C",                                    "L", "F",                     "N",      };
            var list31 = new List<string> {                                                                                           "O", };
            var list26 = new List<string> {                               "P",           "Q", "K",                                         };
            var list27 = new List<string> {                                         "R", "Q",                                              };
            var list28 = new List<string> {                                    "S", "R",                                                   };
            
            lw.Add(list11);
            lw.Add(list21);
            lw.Add(list22);
            lw.Add(list23);
            lw.Add(list24);
            lw.Add(list25);
            lw.Add(list31);
            lw.Add(list26);
            lw.Add(list27);
            lw.Add(list28);
            var result = lw.Sort();

            // Assert
            Assert.AreEqual(sorted, result);
        }
    }
}
