namespace Orc.Sort.Tests.TopologicalSort
{
    using System.Collections.Generic;

    using NUnit.Framework;

    using Orc.Sort.TopologicalSort;

    [TestFixture]
    public class TopologicalSortTests
    {
        [Test]
        public void CanSort_FirstSequenceToBeIncluded_ReturnsTrue()
        {
            // Arrange
            var tp = new TopologicalSort<string>();
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
            var tp = new TopologicalSort<string>();
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
            var tp = new TopologicalSort<string>();
            var seq1 = new List<string> { "A", "B", "C", };

            // Act
            tp.Add(seq1);
            bool result = tp.CanSort(null);

            // Assert
            Assert.False(result);
        }

        [Test]
        public void CanSort_LoopInSequenceIsNOTSortable_ReturnsFalse()
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
        public void CanSort_SecondListIsSortable_ReturnsTrue()
        {
            // Arrange
            var tp = new TopologicalSort<string>();
            var seq1 = new List<string> { "A", "B", "C", };
            var seq2 = new List<string> { "A", "D", };

            // Act
            tp.Add(seq1);
            bool result = tp.CanSort(seq2);

            // Assert
            Assert.True(result);
        }

        [Test]
        public void CanSort_ThirdSequenceIsNOTSortable_ReturnsFalse()
        {
            // Arrange
            var tp = new TopologicalSort<string>();
            var seq1 = new List<string> { "A", "B", };
            var seq2 = new List<string> { "A", "C", "D", };
            var seq3 = new List<string> { "D", "A", };

            // Act
            tp.Add(seq1);
            tp.Add(seq2);
            bool result = tp.CanSort(seq3);

            // Assert
            Assert.False(result);
        }

        [Test]
        public void CanSort_SecondSequenceIsNOTSortable_ReturnsFalse()
        {
            // Arrange
            var tp = new TopologicalSort<string>();
            var seq1 = new List<string> { "A", "B", "C", };
            var seq2 = new List<string> { "C", "A", };

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
            var tp = new TopologicalSort<string>();
            var seq1 = new List<string> { "A", "B" };
            var seq2 = new List<string> { "C", "A" };
            var seq3 = new List<string> { "D", "B", "C" };

            // Act
            tp.Add(seq1);
            tp.Add(seq2);
            bool result = tp.CanSort(seq3);

            // Due to list1 and list2, C is before B (C <- A <- B).
            // seq3 therefore introduces a conflict (C -> B).

            // Assert
            Assert.False(result);
        }

        [Test]
        public void Sort_LoopInSequenceIsNOTSortable_ReturnsNull()
        {
            // Arrange
            var tp = new TopologicalSort<string>();
            var seq1 = new List<string> { "A", "B", "C", "A", };

            // Act
            tp.Add(seq1);
            var result = tp.Sort();

            // Assert
            Assert.IsNull(result);
        }

        [Test]
        public void Sort_SecondSequenceIsNOTSortable_ReturnsNull()
        {
            // Arrange
            var tp = new PriorityTopologicalSort<string>();
            var seq1 = new List<string> { "A", "B", "C", };
            var seq2 = new List<string> { "C", "A", };

            // Act
            tp.Add(seq1);
            tp.Add(seq2);
            var result = tp.Sort();

            // Assert
            Assert.IsNull(result);
        }

        [Test]
        public void Sort_CollectionOfSequences_ReturnsCorrectSequence()
        {
            // Arrange
            var tp = new TopologicalSort<string>();

            var seq1 = new List<string> { "A", "B", "C", };
            var seq2 = new List<string> { "B", "D", "E", };

            // Act
            tp.Add(seq1);
            tp.Add(seq2);
            var result = tp.Sort();

            // Assert
            Assert.True("A".ComesBefore("D", result));
            Assert.True("A".ComesBefore("E", result));
            Assert.True("B".ComesBefore("C", result));
        }

        [Test]
        public void Sort_CollectionOfSequences_ReturnsCorrectSequence2()
        {
            // Arrange
            var tp = new TopologicalSort<string>();

            var seq11 = new List<string> { "A", };
            var seq21 = new List<string> { "B", "C", "D", };
            var seq22 = new List<string> { "E", "B", "F", "G", };
            var seq23 = new List<string> { "C", "G", "H", };
            var seq24 = new List<string> { "B", "I", "H", };

            // Act
            tp.Add(seq11);
            tp.Add(seq21);
            tp.Add(seq22);
            tp.Add(seq23);
            tp.Add(seq24);

            var result = tp.Sort();

            // Assert
            // { "A", "E", "B", "C", "D", "F", "G", "I", "H"};
            Assert.True("E".ComesBefore("C", result));
            Assert.True("E".ComesBefore("D", result));
            Assert.True("E".ComesBefore("I", result));
            Assert.True("E".ComesBefore("H", result));
            Assert.True("B".ComesBefore("G", result));
            Assert.True("B".ComesBefore("H", result));
        }

        [Test]
        public void Sort_CollectionOfSequences_ReturnsCorrectSequence5()
        {
            // Arrange
            var tp = new TopologicalSort<string>();

            var sorted = new List<string> { "A", "E", "F", "G", "B", "C", "D", };

            var seq11 = new List<string> { "A", "B", };
            var seq12 = new List<string> { "E", "F", };
            var seq13 = new List<string> { "F", "G", };
            var seq14 = new List<string> { "G", "B", };

            // Act
            tp.Add(seq11);
            tp.Add(seq12);
            tp.Add(seq13);
            tp.Add(seq14);
            var result = tp.Sort();

            // Assert
            Assert.True("F".ComesBefore("B", result));
            Assert.True("E".ComesBefore("G", result));
        }

        [Test]
        public void Sort_CollectionOfSequences_ReturnsCorrectSequence8()
        {
            // Arrange
            var tp = new PriorityTopologicalSort<string>();

            var sorted = new List<string> { "A", "B", "C", "D", "E", };

            var seq11 = new List<string> { "A", };
            var seq12 = new List<string> { "A", "B", };
            var seq13 = new List<string> { "A", "B", "C", };
            var seq14 = new List<string> { "B", "C", "D", "E", };

            // Act
            tp.Add(seq11);
            tp.Add(seq12);
            tp.Add(seq13);
            tp.Add(seq14);
            var result = tp.Sort();

            // Assert
            Assert.AreEqual(sorted, result);
        }
    }
}
