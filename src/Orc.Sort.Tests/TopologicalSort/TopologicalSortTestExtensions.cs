namespace Orc.Sort.Tests.TopologicalSort
{
    using System;
    using System.Collections.Generic;
    using System.Linq;

    public static class TopologicalSortTestExtensions
    {
        public static bool ComesBefore<T>(this T beforeItem, IEnumerable<T> afterItems , IEnumerable<T> collection)
        {
            var beforeItemFound = false;

            var hashedAfterItems = new HashSet<T>(afterItems);

            foreach (var item in collection)
            {
                if (beforeItem.Equals(item))
                {
                    beforeItemFound = true;
                }

                if (hashedAfterItems.Contains(item))
                {
                    if (!beforeItemFound)
                    {
                        return false;
                    }
                }
            }

            if (!beforeItemFound)
            {
                throw new ArgumentException("The beforeItem: " + beforeItem.ToString() + " was not found in the collection.");
            }

            return true;
        }

        public static bool ComesBefore<T>(this T beforeItem, T afterItem, IEnumerable<T> collection)
        {
            return beforeItem.ComesBefore(new List<T>() { afterItem }, collection);
        }

        public static bool ComesAfter<T>(this T afterItem, IEnumerable<T> beforeItems, IEnumerable<T> collection)
        {
            return afterItem.ComesBefore(beforeItems, collection.Reverse());
        }

        public static bool ComesAfter<T>(this T afterItem, T beforeItem, IEnumerable<T> collection)
        {
            return afterItem.ComesAfter(new List<T>() { beforeItem }, collection);
        }
    }
}