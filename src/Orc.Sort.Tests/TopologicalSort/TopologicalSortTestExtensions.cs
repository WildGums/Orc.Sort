namespace Orc.Sort.Tests.TopologicalSort
{
    using System;
    using System.Collections.Generic;

    public static class TopologicalSortTestExtensions
    {
        public static bool ComesBefore<T>(this T item1, T item2, IEnumerable<T> collection)
        {
            var firstItemFound = false;
            var secondItemFound = false;

            foreach (var item in collection)
            {
                if (item1.Equals(item))
                {
                    firstItemFound = true;
                }

                if (item2.Equals(item))
                {
                    secondItemFound = true;

                    if (firstItemFound)
                    {
                        return true;
                    }
                    else
                    {
                        return false;
                    }
                }
            }

            if (!secondItemFound)
            {
                throw new ArgumentException("The second item: " + item2.ToString() + " is not found in the collection.");
            }

            return false;
        } 
    }
}