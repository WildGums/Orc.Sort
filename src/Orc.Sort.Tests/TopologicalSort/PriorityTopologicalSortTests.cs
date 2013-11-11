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
            var list1 = new List<string> { "Blue", "Orange", "Green" };

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
            var list1 = new List<string> { "Blue", "Orange", "Green" };
            var list2 = new List<string>();

            // Act
            lw.SubLists.Add(list1);
            bool result = lw.CanSort(list2);

            // Assert
            Assert.True(result);
        }

        [Test]
        public void CanWeave_NullList_ReturnsFalse()
        {
            // Arrange
            var lw = new PriorityTopologicalSort<string>();
            var list1 = new List<string> { "Blue", "Orange", "Green" };

            // Act
            lw.SubLists.Add(list1);
            bool result = lw.CanSort(null);

            // Assert
            Assert.False(result);
        }

        [Test]
        public void CanWeave_LoopInListIsNOTWeaveable_ReturnsFalse()
        {
            // Arrange
            var lw = new PriorityTopologicalSort<string>();
            var list1 = new List<string> { "Blue", "Orange", "Green", "Orange" };

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
            var list1 = new List<string> { "Blue", "Orange", "Green" };
            var list2 = new List<string> { "Blue", "Gray"};

            // Act
            lw.SubLists.Add(list1);
            bool result = lw.CanSort(list2);

            // Assert
            Assert.True(result);
        }

        [Test]
        public void CanWeave_ThirdListIsNOTWeaveable_ReturnsFalse()
        {
            // Arrange
            var lw = new PriorityTopologicalSort<string>();
            var list1 = new List<string> { "Blue", "Gray" };
            var list2 = new List<string> { "Blue", "Orange", "Green" };
            var list3 = new List<string> { "Green", "Blue" };

            // Act
            lw.SubLists.Add(list1);
            lw.SubLists.Add(list2);
            bool result = lw.CanSort(list3);

            // Assert
            Assert.False(result);
        }

        [Test]
        public void CanWeave_SecondListIsNOTWeaveable_ReturnsFalse()
        {
            // Arrange
            var lw = new PriorityTopologicalSort<string>();
            var list1 = new List<string> { "Blue", "Orange", "Green" };
            var list2 = new List<string> { "Green", "Blue" };

            // Act
            lw.SubLists.Add(list1);
            bool result = lw.CanSort(list2);

            // Assert
            Assert.False(result);
        }

        [Test]
        public void CanWeave_CollectionOfLists_ReturnsCorrectSequence()
        {
            // Arrange
            var lw = new PriorityTopologicalSort<string>();
            var list1 = new List<string> { "Green", "Blue", "Yellow" };
            var list2 = new List<string> { "Blue", "Orange", "Red" };

            // Act
            lw.SubLists.Add(list1);
            lw.SubLists.Add(list2);
            var result = lw.Sort();

            var correct = new List<string> { "Green", "Blue", "Yellow", "Orange", "Red" };

            // Assert
            Assert.AreEqual(correct, result );
        }

        [Test]
        public void CanWeave_IndirectLoop_ReturnsFalse_RdW()
        {
            // Arrange
            var lw = new PriorityTopologicalSort<string>();
            var list1 = new List<string> { "Purple", "Yellow" };
            var list2 = new List<string> { "Black", "Purple" };
            var list3 = new List<string> { "Blue", "Yellow", "Black" };

            // Act
            lw.SubLists.Add(list1);
            lw.SubLists.Add(list2);
            bool result = lw.CanSort(list3);

            // Due to list1 and list2, Black is before Yellow (Black->Purple->Yellow).
            // list3 therefore introduces a conflict (Yellow before Black).

            // Assert
            Assert.False(result);
        }

        [Test]
        public void CanWeave_CollectionOfLists_ReturnsCorrectSequence2()
        {
            // Arrange
            var lw = new PriorityTopologicalSort<string>();
            var list0 = new List<string> { "Beige" };
            var list1 = new List<string> { "Yellow", "Orange", "Gray"};
            var list2 = new List<string> { "Purple", "Yellow", "Green", "Red" };
            var list3 = new List<string> { "Orange", "Red", "Cyan" };
            var list4 = new List<string> { "Yellow", "Brown", "Cyan" };
            var list5 = new List<string> { "Gold", "Silver", "Bronze" };

            // Act
            lw.SubLists.Add(list0);
            lw.SubLists.Add(list1);
            lw.SubLists.Add(list2);
            lw.SubLists.Add(list3);
            lw.SubLists.Add(list4);
            lw.SubLists.Add(list5);
            var result = lw.Sort();

            var correct = new List<string> { "Beige", "Purple", "Yellow", "Orange", "Gray", "Green", "Red", "Brown", "Cyan", "Gold", "Silver" , "Bronze"};

            // Assert
            Assert.AreEqual(correct, result);
        }

        [Test]
        public void CanWeave_CollectionOfLists_ReturnsCorrectSequence3()
        {
            // Arrange
            var lw = new PriorityTopologicalSort<string>();
            var list0 = new List<string> { "Beige" };
            var list1 = new List<string> { "Yellow", "Orange", "Gray" };
            var list2 = new List<string> { "Purple", "Yellow", "Green", "Red" };
            var list3 = new List<string> { "Orange", "Red", "Cyan" };
            var list4 = new List<string> { "Gold", "Silver", "Bronze" };
            var list5 = new List<string> { "Yellow", "Brown", "Cyan" };
            

            // Beige
            //                Yellow, Orange, Gray
            //        Purple, Yellow               Green, Red,
            //                        Orange,             Red,       Cyan
            //                                                            Gold, Silver, Bronze
            //                Yellow,                         Brown, Cyan


            // Act
            lw.SubLists.Add(list0);
            lw.SubLists.Add(list1);
            lw.SubLists.Add(list2);
            lw.SubLists.Add(list3);
            lw.SubLists.Add(list4);
            lw.SubLists.Add(list5);
            var result = lw.Sort();

            var correct = new List<string> { "Beige", "Purple", "Yellow", "Orange", "Gray", "Green", "Red", "Brown", "Cyan", "Gold", "Silver", "Bronze" };

            // Assert
            Assert.AreEqual(correct, result);
        }

        [Test]
        public void CanWeave_CollectionOfLists_ReturnsCorrectSequence4()
        {
            // Arrange
            var lw = new PriorityTopologicalSort<string>();
            var list0 = new List<string> { "Beige" };
            var list1 = new List<string> { "Yellow", "Orange", "Gray" };
            var list2 = new List<string> { "Yellow", "Brown", "Cyan" };
            var list3 = new List<string> { "Gold", "Silver", "Bronze" };
            var list4 = new List<string> { "Purple", "Yellow", "Green", "Red", "Black" };
            var list5 = new List<string> { "Orange", "Red", "Cyan", "Aqua" };
           

            // Beige
            //               Yellow, Orange, Gray
            //               Yellow,               Borwn,             Cyan
            //                                                              Gold, Silver, Bronze
            //       Purple, Yellow,                     Green, Red                               Black
            //                       Orange,                    Red,  Cyan                               Aqua

            // Act
            lw.SubLists.Add(list0);
            lw.SubLists.Add(list1);
            lw.SubLists.Add(list2);
            lw.SubLists.Add(list3);
            lw.SubLists.Add(list4);
            lw.SubLists.Add(list5);
            var result = lw.Sort();

            var correct = new List<string> { "Beige", "Purple", "Yellow", "Orange", "Gray" ,"Brown", "Green", "Red", "Cyan", "Gold", "Silver", "Bronze", "Black", "Aqua" };

            // Assert
            Assert.AreEqual(correct, result);
        }

        [Test]
        public void CanWeave_CollectionOfLists_ReturnsCorrectSequence5()
        {
            // Arrange
            var lw = new PriorityTopologicalSort<string>();
            var list0 = new List<string> { "Beige", "Black" };
            var list1 = new List<string> { "Orange", "Gray" };
            var list2 = new List<string> { "Yellow", "Gold" };
            var list3 = new List<string> { "Gold", "Red" };
            var list4 = new List<string> { "Red", "Black" };

            // Beige,                   Black
            //                                 Orange, Gray
            //       Yellow, Gold
            //               Gold, Red,
            //                     Red, Black


            // Act
            lw.SubLists.Add(list0);
            lw.SubLists.Add(list1);
            lw.SubLists.Add(list2);
            lw.SubLists.Add(list3);
            lw.SubLists.Add(list4);
            var result = lw.Sort();

            var correct = new List<string> { "Beige", "Yellow", "Gold", "Red", "Black", "Orange", "Gray", };

            // Assert
            Assert.AreEqual(correct, result);
        }

        [Test]
        public void CanWeave_CollectionOfLists_ReturnsCorrectSequence6()
        {
            // Arrange
            var lw = new PriorityTopologicalSort<string>();
            var list0 = new List<string> { "Beige", "Black" };
            var list1 = new List<string> { "Orange", "Gray" };
            var list2 = new List<string> { "Yellow", "Gold" };
            var list3 = new List<string> { "Gold", "Red" };
            var list4 = new List<string> { "Orange", "Black" };
            var list5= new List<string> { "Red", "Black" };

            // Beige,                           Black
            //       Orange,                          Gray
            //               Yellow, Gold
            //                       Gold, Red,
            //      Orange,                     Black  
            //                             Red, Black


            // Act
            lw.SubLists.Add(list0);
            lw.SubLists.Add(list1);
            lw.SubLists.Add(list2);
            lw.SubLists.Add(list3);
            lw.SubLists.Add(list4);
            lw.SubLists.Add(list5);
            var result = lw.Sort();

            var correct = new List<string> { "Beige", "Orange", "Yellow", "Gold", "Red", "Black", "Gray", };

            // Assert
            Assert.AreEqual(correct, result);
        }

        [Test]
        public void CanWeave_CollectionOfLists_ReturnsCorrectSequence7()
        {
            // Arrange
            var lw = new PriorityTopologicalSort<string>();
            var list0 = new List<string> { "Blue" };
            var list1 = new List<string> { "Gold" };
            var list2 = new List<string> { "Orange", "Yellow", "Blue" };
            var list3 = new List<string> { "Brown", "Blue" };

            // Act
            lw.SubLists.Add(list0);
            lw.SubLists.Add(list1);
            lw.SubLists.Add(list2);
            lw.SubLists.Add(list3);
            var result = lw.Sort();

            var correct = new List<string> { "Orange", "Yellow", "Brown", "Blue", "Gold" };

            // Assert
            Assert.AreEqual(correct, result);
        }

        [Test]
        public void CanWeave_CollectionOfLists_ReturnsCorrectSequenceRdW()
        {
            // Arrange
            var lw = new PriorityTopologicalSort<string>();
            var list1 = new List<string> { "Blue" };
            var list2 = new List<string> { "Bronze", "Yellow", "Purple" };
            var list3 = new List<string> { "Black", "Orange" };
            var list4 = new List<string> { "Red", "Blue", "Gold" };
            var list5 = new List<string> { "Silver", "Blue" };
            var list6 = new List<string> { "Bronze", "Blue" };

            // Act
            lw.SubLists.Add(list1);
            lw.SubLists.Add(list2);
            lw.SubLists.Add(list3);
            lw.SubLists.Add(list4);
            lw.SubLists.Add(list5);
            lw.SubLists.Add(list6);
            var result = lw.Sort();

            var correct = new List<string> { "Bronze", "Red", "Silver", "Blue", "Yellow", "Purple", "Black", "Orange", "Gold" };

            // Assert
            Assert.AreEqual(correct, result);
        }

        [Test]
        public void CanWeave_CollectionOfLists_ReturnsCorrectSequenceRdW2()
        {
            // Arrange
            var lw = new PriorityTopologicalSort<string>();
            var list1 = new List<string> { "Purple" };
            var list2 = new List<string> { "Green" };
            var list3 = new List<string> { "Orange", "Purple"};
            var list4 = new List<string> { "Green", "Purple" };

            // Act
            lw.SubLists.Add(list1);
            lw.SubLists.Add(list2);
            lw.SubLists.Add(list3);
            lw.SubLists.Add(list4);
            var result = lw.Sort();

            var correct = new List<string> { "Green", "Orange", "Purple"};

            // Assert
            Assert.AreEqual(correct, result);
        }

        [Test]
        public void CanWeave_CollectionOfLists_ReturnsCorrectSequenceRdW3()
        {
            // Arrange
            var lw = new PriorityTopologicalSort<string>();
            var list0 = new List<string> { "Yellow" };
            var list1 = new List<string> { "Blue", "Black", "Orange" };
            var list2 = new List<string> { "Red", "Yellow" };
            var list3 = new List<string> { "Black", "Bronze", "Yellow" };

            // Act
            lw.SubLists.Add(list0);
            lw.SubLists.Add(list1);
            lw.SubLists.Add(list2);
            lw.SubLists.Add(list3);
            var result = lw.Sort();

            var correct = new List<string> { "Blue", "Black", "Red", "Bronze", "Yellow", "Orange" };

            // Assert
            Assert.AreEqual(correct, result);
        }


        [Test]
        public void CanWeave_CollectionOfLists_ReturnsCorrectSequence_M1()
        {
            // Arrange
            var lw = new PriorityTopologicalSort<string>();
            var list0 = new List<string> { "Beige" };
            var list1 = new List<string> { "Yellow", "Orange", "Gray" };
            var list2 = new List<string> { "Yellow", "Brown", "Cyan" };
            var list3 = new List<string> { "Gold", "Silver", "Bronze" };
            var list4 = new List<string> { "Purple", "Yellow", "Green", "Red", "Black" };
            var list5 = new List<string> { "Orange", "Red", "Cyan", "Aqua" };
            var list6 = new List<string> { "Problem", "Green" };
            var list7 = new List<string> { "Problem2", "Problem" };

            lw.SubLists.Add(list0);
            lw.SubLists.Add(list1);
            lw.SubLists.Add(list2);
            lw.SubLists.Add(list3);
            lw.SubLists.Add(list4);
            lw.SubLists.Add(list5);
            lw.SubLists.Add(list6);
            lw.SubLists.Add(list7);
            var result = lw.Sort();
            var correct = new List<string> { "Beige", "Purple", "Yellow", "Orange", "Gray", "Brown", "Problem2", "Problem", "Green", "Red", "Cyan", "Gold", "Silver", "Bronze", "Black", "Aqua" };
            // Assert
            Assert.AreEqual(correct, result);
        }
        [Test]
        public void CanWeave_CollectionOfLists_ReturnsCorrectSequence_M2()
        {
            // Arrange
            var lw = new PriorityTopologicalSort<string>();
            var list0 = new List<string> { "Beige" };
            var list1 = new List<string> { "Yellow", "Orange", "Gray" };
            var list2 = new List<string> { "Yellow", "Brown", "Cyan" };
            var list3 = new List<string> { "Gold", "Silver", "Bronze" };
            var list4 = new List<string> { "Purple", "Yellow", "Green", "Red", "Black" };
            var list5 = new List<string> { "Orange", "Red", "Cyan", "Aqua" };
            var list6 = new List<string> { "Additional", "Problem", "Green" };
            var list7 = new List<string> { "Problem2", "Problem" };
            var list8 = new List<string> { "Additional2", "Problem2" };

            lw.SubLists.Add(list0);
            lw.SubLists.Add(list1);
            lw.SubLists.Add(list2);
            lw.SubLists.Add(list3);
            lw.SubLists.Add(list4);
            lw.SubLists.Add(list5);
            lw.SubLists.Add(list6);
            lw.SubLists.Add(list7);
            lw.SubLists.Add(list8);
            var result = lw.Sort();
            var correct = new List<string> { "Beige", "Purple", "Yellow", "Orange", "Gray", "Brown", "Additional", "Additional2", "Problem2", "Problem", "Green", "Red", "Cyan", "Gold", "Silver", "Bronze", "Black", "Aqua" };
            // Assert
            Assert.AreEqual(correct, result);
        }
    }
}
