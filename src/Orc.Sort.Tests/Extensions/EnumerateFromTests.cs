// --------------------------------------------------------------------------------------------------------------------
// <copyright file="EnumerateFromTests.cs" company="Orcomp development team">
//   Copyright (c) 2008 - 2015 Orcomp development team. All rights reserved.
// </copyright>
// --------------------------------------------------------------------------------------------------------------------


namespace Orc.Sort.Tests
{
    using System;
    using System.Collections.Generic;
    using System.Linq;
    using NUnit.Framework;

    [TestFixture]
    public class EnumerateFrom
    {
        #region Methods
        [Test]
        public void EnumerateFrom_UniqueValues_StartFrom_A()
        {
            var collection = new List<string>() {"A", "B", "C", "D", "E", "F", "G"};

            var result = collection.EnumerateFrom("A");

            var expected = new List<string>() {"A", "B", "C", "D", "E", "F", "G"};

            Assert.AreEqual(expected, result);
        }

        [Test]
        public void EnumerateFrom_UniqueValues_StartFrom_C()
        {
            var collection = new List<string>() {"A", "B", "C", "D", "E", "F", "G"};

            var result = collection.EnumerateFrom("C");

            var expected = new List<string>() {"C", "D", "E", "F", "G", "A", "B"};

            Assert.AreEqual(expected, result);
        }

        [Test]
        public void EnumerateFrom_UniqueValues_StartFrom_C_NonCyclical()
        {
            var collection = new List<string>() {"A", "B", "C", "D", "E", "F", "G"};

            var result = collection.EnumerateFrom("C", false);

            var expected = new List<string>() {"C", "D", "E", "F", "G"};

            Assert.AreEqual(expected, result);
        }

        [Test]
        public void EnumerateFrom_UniqueValues_StartFrom_G_NonCyclical()
        {
            var collection = new List<string>() {"A", "B", "C", "D", "E", "F", "G"};

            var result = collection.EnumerateFrom("G", false);

            var expected = new List<string>() {"G"};

            Assert.AreEqual(expected, result);
        }

        [Test]
        public void EnumerateFrom_NonUniqueValues_StartFrom_A_ThrowException()
        {
            var collection = new List<string>() {"A", "B", "C", "A", "E", "F", "G"};

            Assert.Throws<ArgumentException>(() => collection.EnumerateFrom("A").ToArray());
        }

        [Test]
        public void EnumerateFrom_StartValueDoeNotExist_StartFrom_X_ThrowException()
        {
            var collection = new List<string>() {"A", "B", "C", "A", "E", "F", "G"};

            Assert.Throws<ArgumentException>(() => collection.EnumerateFrom("X").ToArray());
        }
        #endregion
    }
}