namespace Orc.Sort.Tests.TopologicalSort
{
    using System.Collections.Generic;

    using NUnit.Framework;

    using Orc.Sort.TopologicalSort;

    [TestFixture]
    public class PriorityTopologicalSortTests
    {
        [Test]
        public void CanSort_FirstSequenceToBeIncluded_ReturnsTrue()
        {
            // Arrange
            var tp = new PriorityTopologicalSort<string>();
            var seq1 = new List<string> { "A", "B", "C", };

            // Act
            bool result = tp.CanSort(seq1);

            // Assert
            Assert.True(result);
        }

        [Test]
        public void CanSort_EmptySequence_ReturnsTrue()
        {
            // Arrange
            var tp = new PriorityTopologicalSort<string>();
            var seq1 = new List<string> { "A", "B", "C", };
            var seq2 = new List<string>();

            // Act
            tp.Add(seq1);
            bool result = tp.CanSort(seq2);

            // Assert
            Assert.True(result);
        }

        [Test]
        public void CanSort_NullSequence_ReturnsFalse()
        {
            // Arrange
            var tp = new PriorityTopologicalSort<string>();
            var seq1 = new List<string> { "A", "B", "C", };

            // Act
            tp.Add(seq1);
            bool result = tp.CanSort(null);

            // Assert
            Assert.False(result);
        }

        [Test]
        public void CanSort_LoopInSequenceIsNOTWeaveable_ReturnsFalse()
        {
            // Arrange
            var tp = new PriorityTopologicalSort<string>();
            var seq1 = new List<string> { "A", "B", "C", "A", };

            // Act
            bool result = tp.CanSort(seq1);

            // Assert
            Assert.False(result);
        }

        [Test]
        public void CanSort_SecondSequenceIsWeaveable_ReturnsTrue()
        {
            // Arrange
            var tp = new PriorityTopologicalSort<string>();
            var seq1 = new List<string> { "A", "B", "C", };
            var seq2 = new List<string> { "A", "D",      };

            // Act
            tp.Add(seq1);
            bool result = tp.CanSort(seq2);

            // Assert
            Assert.True(result);
        }

        [Test]
        public void CanSort_ThirdSequenceIsNOTWeaveable_ReturnsFalse()
        {
            // Arrange
            var tp = new PriorityTopologicalSort<string>();
            var seq1 = new List<string> { "A", "B",      };
            var seq2 = new List<string> { "A", "C", "D", };
            var seq3 = new List<string> { "D", "A",      };

            // Act
            tp.Add(seq1);
            tp.Add(seq2);
            bool result = tp.CanSort(seq3);

            // Assert
            Assert.False(result);
        }

        [Test]
        public void CanSort_SecondSequenceIsNOTWeaveable_ReturnsFalse()
        {
            // Arrange
            var tp = new PriorityTopologicalSort<string>();
            var seq1 = new List<string> { "A", "B", "C", };
            var seq2 = new List<string> { "C", "A",      };

            // Act
            tp.Add(seq1);
            bool result = tp.CanSort(seq2);

            // Assert
            Assert.False(result);
        }

        [Test]
        public void CanSort_IndirectLoop_ReturnsFalse_RdW()
        {
            // Arrange
            var tp = new PriorityTopologicalSort<string>();
            var seq1 = new List<string> { "A", "B" };
            var seq2 = new List<string> { "C", "A" };
            var seq3 = new List<string> { "D", "B", "C" };

            // Act
            tp.Add(seq1);
            tp.Add(seq2);
            bool result = tp.CanSort(seq3);

            // Due to seq1 and seq2, C is before B (C <- A <- B).
            // seq3 therefore introduces a conflict (C -> B).

            // Assert
            Assert.False(result);
        }

        [Test]
        public void Sort_CollectionOfSequences_ReturnsCorrectSequence()
        {
            // Arrange
            var tp = new PriorityTopologicalSort<string>();

            var sort0 = new List<string> { "A", "B", "C", "D", "E", };
            var seq1 = new List<string> { "A", "B", "C",           };
            var seq2 = new List<string> {      "B",      "D", "E", };

            // Act
            tp.Add(seq1);
            tp.Add(seq2);
            var result = tp.Sort();

            // Assert
            Assert.AreEqual(sort0, result );
        }

        [Test]
        public void Sort_CollectionOfSequences_ReturnsCorrectSequence2()
        {
            // Arrange
            var tp = new PriorityTopologicalSort<string>();

            var sorted = new List<string> { "A", "E", "B", "C", "D", "F", "G", "I", "H", "J", "K", "L", };

            var seq11 = new List<string> { "A",                                                        };
            var seq21 = new List<string> {           "B", "C", "D",                                    };
            var seq22 = new List<string> {      "E", "B",           "F", "G",                          };
            var seq23 = new List<string> {                "C",           "G",      "H",                };
            var seq24 = new List<string> {           "B",                     "I", "H",                };
            var seq31 = new List<string> {                                              "J", "K", "L", };

            // Act
            tp.Add(seq11);
            tp.Add(seq21);
            tp.Add(seq22);
            tp.Add(seq23);
            tp.Add(seq24);
            tp.Add(seq31);
            var result = tp.Sort();

            // Assert
            Assert.AreEqual(sorted, result);
        }

        [Test]
        public void Sort_CollectionOfSequences_ReturnsCorrectSequence3()
        {
            // Arrange
            var tp = new PriorityTopologicalSort<string>();

            var sort0 = new List<string> { "A", "E", "B", "C", "D", "F", "G", "L", "H", "I", "J", "K", };

            var seq0 = new List<string> { "A",                                                        };
            var seq1 = new List<string> {           "B", "C", "D",                                    };
            var seq2 = new List<string> {      "E", "B",           "F", "G",                          };
            var seq3 = new List<string> {                "C",           "G",      "H",                };
            var seq4 = new List<string> {                                              "I", "J", "K", };
            var seq5 = new List<string> {           "B",                     "L", "H",                };
            
            // Act
            tp.Add(seq0);
            tp.Add(seq1);
            tp.Add(seq2);
            tp.Add(seq3);
            tp.Add(seq4);
            tp.Add(seq5);
            var result = tp.Sort();

            // Assert
            Assert.AreEqual(sort0, result);
        }

        [Test]
        public void Sort_CollectionOfSequences_ReturnsCorrectSequence4()
        {
            // Arrange
            var tp = new PriorityTopologicalSort<string>();

            var sorted = new List<string> { "A", "J", "B", "C", "D", "E", "K", "L", "F", "G", "H", "I", "M", "N", };

            var seq11 = new List<string> { "A",                                                                  };
            var seq21 = new List<string> {           "B", "C", "D",                                              };
            var seq22 = new List<string> {           "B",           "E",           "F",                          };
            var seq31 = new List<string> {                                              "G", "H", "I",           };
            var seq23 = new List<string> {      "J", "B",                "K", "L",                     "M",      };
            var seq24 = new List<string> {                "C",                "L", "F",                     "N", };
           
            // Act
            tp.Add(seq11);
            tp.Add(seq21);
            tp.Add(seq22);
            tp.Add(seq31);
            tp.Add(seq23);
            tp.Add(seq24);
            var result = tp.Sort();

            // Assert
            Assert.AreEqual(sorted, result);
        }

        [Test]
        public void Sort_CollectionOfSequences_ReturnsCorrectSequence5()
        {
            // Arrange
            var tp = new PriorityTopologicalSort<string>();

            var sorted = new List<string> { "A", "E", "F", "G", "B", "C", "D", };

            var seq11 = new List<string> { "A",                "B",           };
            var seq21 = new List<string> {                          "C", "D", };
            var seq12 = new List<string> {      "E", "F",                     };
            var seq13 = new List<string> {           "F", "G",                };
            var seq14 = new List<string> {                "G", "B",           };

            // Act
            tp.Add(seq11);
            tp.Add(seq21);
            tp.Add(seq12);
            tp.Add(seq13);
            tp.Add(seq14);
            var result = tp.Sort();

            // Assert
            Assert.AreEqual(sorted, result);
        }

        [Test]
        public void Sort_CollectionOfSequences_ReturnsCorrectSequence6()
        {
            // Arrange
            var tp = new PriorityTopologicalSort<string>();

            var sorted = new List<string> { "A", "C", "E", "F", "G", "B", "D", };

            var seq11 = new List<string> { "A",                     "B",      };
            var seq12 = new List<string> {      "C",                     "D", };
            var seq13 = new List<string> {           "E", "F",                };
            var seq14 = new List<string> {                "F", "G",           };
            var seq15 = new List<string> {      "C",                "B",      };
            var seq16 = new List<string> {                     "G", "B",      };
            
            // Act
            tp.Add(seq11);
            tp.Add(seq12);
            tp.Add(seq13);
            tp.Add(seq14);
            tp.Add(seq15);
            tp.Add(seq16);
            var result = tp.Sort();
            
            // Assert
            Assert.AreEqual(sorted, result);
        }

        [Test]
        public void Sort_CollectionOfSequences_ReturnsCorrectSequence7()
        {
            // Arrange
            var tp = new PriorityTopologicalSort<string>();

            var sorted = new List<string> { "C", "D", "E", "A", "B", };

            var seq11 = new List<string> {                "A",      };
            var seq21 = new List<string> {                     "B", };
            var seq12 = new List<string> { "C", "D",      "A",      };
            var seq13 = new List<string> {           "E", "A",      };

            // Act
            tp.Add(seq11);
            tp.Add(seq21);
            tp.Add(seq12);
            tp.Add(seq13);
            var result = tp.Sort();

            // Assert
            Assert.AreEqual(sorted, result);
        }

        [Test]
        public void Sort_CollectionOfSequences_ReturnsCorrectSequence8()
        {
            // Arrange
            var tp = new PriorityTopologicalSort<string>();

            var sorted = new List<string> { "A", "B", "C", "D", "E", };

            var seq11 = new List<string> { "A",                     };
            var seq12 = new List<string> { "A", "B",                };
            var seq13 = new List<string> { "A", "B", "C",           };
            var seq14 = new List<string> {      "B", "C", "D", "E", };

            // Act
            tp.Add(seq11);
            tp.Add(seq12);
            tp.Add(seq13);
            tp.Add(seq14);
            var result = tp.Sort();

            // Assert
            Assert.AreEqual(sorted, result);
        }

        [Test]
        public void Sort_CollectionOfSequences_ReturnsCorrectSequenceRdW()
        {
            // Arrange
            var tp = new PriorityTopologicalSort<string>();

            var sorted = new List<string> { "B", "G", "I", "A", "C", "D", "E", "F", "H", };

            var seq11 = new List<string> {                "A",                          };
            var seq12 = new List<string> { "B",                "C", "D",                };
            var seq13 = new List<string> {                               "E", "F",      };
            var seq14 = new List<string> {      "G",      "A",                     "H", };
            var seq15 = new List<string> {           "I", "A",                          };
            var seq16 = new List<string> { "B",           "A",                          };

            // Act
            tp.Add(seq11);
            tp.Add(seq12);
            tp.Add(seq13);
            tp.Add(seq14);
            tp.Add(seq15);
            tp.Add(seq16);
            var result = tp.Sort();

            // Assert
            Assert.AreEqual(sorted, result);
        }

        [Test]
        public void Sort_CollectionOfSequences_ReturnsCorrectSequenceRdW2()
        {
            // Arrange
            var tp = new PriorityTopologicalSort<string>();

            var sorted = new List<string> { "B", "C", "A", };

            var seq11 = new List<string> {           "A", };
            var seq12 = new List<string> { "B",           };
            var seq13 = new List<string> {      "C", "A", };
            var seq14 = new List<string> { "B",      "A", };

            // Act
            tp.Add(seq11);
            tp.Add(seq12);
            tp.Add(seq13);
            tp.Add(seq14);
            var result = tp.Sort();

            // Assert
            Assert.AreEqual(sorted, result);
        }

        [Test]
        public void Sort_CollectionOfSequences_ReturnsCorrectSequenceRdW3()
        {
            // Arrange
            var tp = new PriorityTopologicalSort<string>();

            var sorted = new List<string> { "B", "C", "E", "F", "A", "D", };
            var seq11 = new List<string> {                     "A",      };
            var seq12 = new List<string> { "B", "C",                "D", };
            var seq13 = new List<string> {           "E",      "A",      };
            var seq14 = new List<string> {      "C",      "F", "A" };

            // Act
            tp.Add(seq11);
            tp.Add(seq12);
            tp.Add(seq13);
            tp.Add(seq14);
            var result = tp.Sort();

            // Assert
            Assert.AreEqual(sorted, result);
        }


        [Test]
        public void Sort_CollectionOfSequences_ReturnsCorrectSequence_M1()
        {
            // Arrange
            var tp = new PriorityTopologicalSort<string>();

            var sorted = new List<string> { "A", "J", "B", "C", "D", "E", "Q", "P", "K", "L", "F", "G", "H", "I", "M", "N", "O", };

            var seq11 = new List<string> { "A",                                                                                 };
            var seq21 = new List<string> {           "B", "C", "D",                                                             };
            var seq22 = new List<string> {           "B",           "E",                     "F",                               };
            var seq23 = new List<string> {                                                        "G", "H", "I",                };
            var seq24 = new List<string> {      "J", "B",                          "K", "L",                     "M",           };
            var seq25 = new List<string> {                "C",                          "L", "F",                     "N",      };
            var seq31 = new List<string> {                                                                                 "O", };
            var seq26 = new List<string> {                                    "P", "K",                                         };
            var seq27 = new List<string> {                               "Q", "P",                                              };

            tp.Add(seq11);
            tp.Add(seq21);
            tp.Add(seq22);
            tp.Add(seq23);
            tp.Add(seq24);
            tp.Add(seq25);
            tp.Add(seq31);
            tp.Add(seq26);
            tp.Add(seq27);
            var result = tp.Sort();

            // Assert
            Assert.AreEqual(sorted, result);
        }
        [Test]
        public void Sort_CollectionOfSequences_ReturnsCorrectSequence_M2()
        {
            // Arrange
            var tp = new PriorityTopologicalSort<string>();


            
            var sorted = new List<string> { "A", "J", "B", "C", "D", "E", "P", "S", "R", "Q", "K", "L", "F", "G", "H", "I", "M", "N", "O", };

            var seq11 = new List<string> { "A",                                                                                           };
            var seq21 = new List<string> {           "B", "C", "D",                                                                       };
            var seq22 = new List<string> {           "B",           "E",                               "F",                               };
            var seq23 = new List<string> {                                                                  "G", "H", "I",                };
            var seq24 = new List<string> {      "J", "B",                                    "K", "L",                     "M",           };
            var seq25 = new List<string> {                "C",                                    "L", "F",                     "N",      };
            var seq31 = new List<string> {                                                                                           "O", };
            var seq26 = new List<string> {                               "P",           "Q", "K",                                         };
            var seq27 = new List<string> {                                         "R", "Q",                                              };
            var seq28 = new List<string> {                                    "S", "R",                                                   };
            
            tp.Add(seq11);
            tp.Add(seq21);
            tp.Add(seq22);
            tp.Add(seq23);
            tp.Add(seq24);
            tp.Add(seq25);
            tp.Add(seq31);
            tp.Add(seq26);
            tp.Add(seq27);
            tp.Add(seq28);
            var result = tp.Sort();

            // Assert
            Assert.AreEqual(sorted, result);
        }
    }
}
