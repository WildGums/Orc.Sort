// --------------------------------------------------------------------------------------------------------------------
// <copyright file="IEnumerableExtensions.cs" company="WildGums">
//   Copyright (c) 2008 - 2017 WildGums. All rights reserved.
// </copyright>
// --------------------------------------------------------------------------------------------------------------------


namespace Orc.Sort
{
    using System;
    using System.Collections;
    using System.Collections.Generic;
    using System.Linq;
    using MoreLinq;

    public static class IEnumerableExtensions
    {
        #region Methods
        /// <summary>
        /// Enumerate through a collection starting from a specified item.
        /// The collection must have unique values.
        /// Example:
        ///  {"A", "B", "C", "D", "E"}.EnumerateFrom("B") == {"B", "C", "D", "E", "A"}
        /// </summary>
        /// <typeparam name="T">Must be equatable</typeparam>
        /// <param name="items">The collection to enumerate over</param>
        /// <param name="startValue">The start value to start enumerating from</param>
        /// <param name="isCyclic">Specify whether to stop once the end of the collection is reached or wrap around and enumerate all the items in the collection</param>
        /// <returns></returns>
        public static IEnumerable<T> EnumerateFrom<T>(this IEnumerable<T> items, T startValue, bool isCyclic = true) where T : IEquatable<T>
        {
            var foundStartValue = false;
            var appendItems = new List<T>();

            foreach (var item in items)
            {
                if (item.Equals(startValue))
                {
                    if (foundStartValue)
                    {
                        throw new ArgumentException("The items must be unique.");
                    }

                    foundStartValue = true;
                }

                if (foundStartValue)
                {
                    yield return item;
                }
                else if (isCyclic)
                {
                    appendItems.Add(item);
                }
            }

            if (!foundStartValue)
            {
                throw new ArgumentException("The start value does not exist in items.");
            }

            if (isCyclic)
            {
                foreach (var appendItem in appendItems)
                {
                    yield return appendItem;
                }
            }
        }

        /// <summary>
        /// Takes a collection of sorted items and merges these collections together in order to return one sorted collection.
        /// </summary>
        /// <typeparam name="T"> Must be comparable. </typeparam>
        /// <param name="sortedEnumerables"></param>
        /// <param name="distinct"></param>
        /// <returns></returns>
        public static IEnumerable<T> MergeSorted<T>(this IEnumerable<IEnumerable<T>> sortedEnumerables, bool distinct = false)
            where T : IComparable<T>
        {
            return sortedEnumerables.MergeSorted(Comparer<T>.Default, distinct);
        }

        /// <summary>
        /// Takes a collection of sorted items and merges these collections together in order to return one sorted collection.
        /// </summary>
        /// <typeparam name="T"></typeparam>
        /// <param name="sortedEnumerables"></param>
        /// <param name="itemComparer"></param>
        /// <param name="distinct"></param>
        /// <returns></returns>
        public static IEnumerable<T> MergeSorted<T>(this IEnumerable<IEnumerable<T>> sortedEnumerables, IComparer<T> itemComparer, bool distinct = false)
        {
            var enumerators = sortedEnumerables.Select(e => e.GetEnumerator()).Where(e => e.MoveNext()).ToList();

            while (enumerators.Count > 0)
            {
                var next_enum = enumerators.MinBy(e => e.Current, itemComparer);
                var next_item = next_enum.Current;

                if (distinct)
                {
                    enumerators.RemoveAll(e => (itemComparer.Compare(e.Current, next_item) == 0) && !e.MoveNext());
                }
                else if (!next_enum.MoveNext())
                {
                    enumerators.Remove(next_enum);
                }

                yield return next_item;
            }
        }

        /// <summary>
        /// Takes a collection of sorted items and merges these collections together in order to return one sorted collection.
        /// 
        /// This version is optimized for a high number of collections to merge (100+).
        /// </summary>
        /// <typeparam name="T"></typeparam>
        /// <param name="sortedEnumerables"></param>
        /// <param name="itemComparer"></param>
        /// <returns></returns>
        public static IEnumerable<T> MergeSortedMany<T>(this IEnumerable<IEnumerable<T>> sortedEnumerables, IComparer<T> itemComparer)
        {
            var sortedKeySet = new SortedSet<KeyedEnumerator<T>>();
            int secondaryKey = 0;

            foreach (var enumerator in sortedEnumerables.Select(e => e.GetEnumerator()))
            {
                if (enumerator.MoveNext())
                {
                    sortedKeySet.Add(new KeyedEnumerator<T>(enumerator, itemComparer, secondaryKey++));
                }
            }

            while (sortedKeySet.Count > 1)
            {
                var minItem = sortedKeySet.Min;
                var hasNext = true;
                sortedKeySet.Remove(minItem);

                while (hasNext && minItem.CompareTo(sortedKeySet.Min) < 0)
                {
                    yield return minItem.Current;
                    hasNext = minItem.MoveNext();
                }

                if (hasNext)
                {
                    sortedKeySet.Add(minItem);
                }
            }

            if (sortedKeySet.Count > 0)
            {
                var min = sortedKeySet.Min;
                var has = true;
                sortedKeySet.Remove(min);

                while (has)
                {
                    yield return min.Current;
                    has = min.MoveNext();
                }
            }
        }
        #endregion
    }

    internal class KeyedEnumerator<T> : IEnumerator<T>, IComparable<KeyedEnumerator<T>>
    {
        #region Constructors
        public KeyedEnumerator(IEnumerator<T> enumerator, IComparer<T> itemComparer, int secondaryKey)
        {
            Enumerator = enumerator;
            ItemComparer = itemComparer;
            SecondaryKey = secondaryKey;
        }
        #endregion

        #region Properties
        public T Current
        {
            get { return Enumerator.Current; }
        }

        object IEnumerator.Current
        {
            get { return Enumerator.Current; }
        }

        public IEnumerator<T> Enumerator { get; private set; }
        public IComparer<T> ItemComparer { get; private set; }
        public int SecondaryKey { get; private set; }
        #endregion

        #region Methods
        public int CompareTo(KeyedEnumerator<T> other)
        {
            var result = ItemComparer.Compare(Current, other.Current);

            if (result == 0)
            {
                result = SecondaryKey.CompareTo(other.SecondaryKey);
            }

            return result;
        }

        public void Dispose()
        {
            Enumerator.Dispose();
        }

        public bool MoveNext()
        {
            return Enumerator.MoveNext();
        }

        public void Reset()
        {
            Enumerator.Reset();
        }
        #endregion
    }
}