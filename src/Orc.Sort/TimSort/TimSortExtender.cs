// ReSharper disable CheckNamespace

namespace System.Linq
{
    using Catel.Reflection;
    using Collections.Generic;
    using Reflection;
    using TimSort;

    /// <summary>
    /// <![CDATA[T[], List<T> and IList<T>]]> extender providing TimSort extension methods.
    /// </summary>
    public static partial class TimSortExtender
    {
        /// <summary>Proxy object to resolve sorter dynamically.</summary>
        internal class SorterProxy
        {
            public SorterProxy(Action<object> sortAll, Action<object, int, int> sortRange)
            {
                SortAll = sortAll;
                SortRange = sortRange;
            }

            /// <summary>The sort all proxy.</summary>
            public Action<object> SortAll;

            /// <summary>The sort range proxy.</summary>
            public Action<object, int, int> SortRange;
        }

        private static readonly Dictionary<Type, SorterProxy> SorterProxyMap
            = new Dictionary<Type, SorterProxy>();

        /// <summary>Determines whether type implements IComparable.</summary>
        /// <typeparam name="T">Any type.</typeparam>
        /// <returns><c>true</c> if  implements IComparable; otherwise, <c>false</c>.</returns>
        private static bool IsIComparable<T>()
        {
            return (typeof(IComparable<T>)).IsAssignableFrom(typeof(T));
        }

        /// <summary>Gets the sorter proxy for IComparable.</summary>
        /// <typeparam name="TContainer">The type of the container.</typeparam>
        /// <typeparam name="TItem">The type of the item.</typeparam>
        /// <param name="sorterType">Type of the sorter.</param>
        /// <returns>Sorter proxy.</returns>
        private static SorterProxy GetComparableSorterProxy<TContainer, TItem>(Type sorterType)
            where TContainer : class
        {
            ArgumentNullException.ThrowIfNull(sorterType);

            var key = sorterType;

            if (SorterProxyMap.TryGetValue(key, out var sorter))
            {
                return sorter;
            }

            const BindingFlags flags = BindingFlags.Static | BindingFlags.NonPublic;

            var staticType = sorterType.MakeGenericType(typeof(TItem));

            var sortAllMethod = staticType.GetMethod("SortAll", flags);
            if (sortAllMethod is null)
            {
                throw new InvalidOperationException($"Cannot find SortAll method on '{typeof(TItem).Name}'");
            }

            var sortAll = (Action<TContainer>)DelegateHelper.CreateDelegate(
                typeof(Action<TContainer>), sortAllMethod);

            var sortRangeMethod = staticType.GetMethod("SortRange", flags);
            if (sortRangeMethod is null)
            {
                throw new InvalidOperationException($"Cannot find SortRange method on '{typeof(TItem).Name}'");
            }

            var sortRange = (Action<TContainer, int, int>)DelegateHelper.CreateDelegate(
                typeof(Action<TContainer, int, int>), sortRangeMethod);
            
            sorter = new SorterProxy((array) => sortAll((TContainer)array),
                (array, lo, hi) => sortRange((TContainer)array, lo, hi));

            SorterProxyMap[key] = sorter;

            return sorter;
        }

        /// <summary>Tries sorting as IComparable.</summary>
        /// <typeparam name="TItem">Item type.</typeparam>
        /// <param name="array">The array.</param>
        /// <returns><c>true</c> if sorting was successful, <c>false</c> otherwise.</returns>
        private static bool TryComparableTimSort<TItem>(TItem[] array)
        {
            ArgumentNullException.ThrowIfNull(array);

            if (!IsIComparable<TItem>())
            {
                return false;
            }

            GetComparableSorterProxy<TItem[], TItem>(typeof(ComparableArrayTimSort<>)).SortAll(array);

            return true;
        }

        /// <summary>Tries sorting as IComparable.</summary>
        /// <typeparam name="TItem">The type of the item.</typeparam>
        /// <param name="array">The array.</param>
        /// <param name="lo">The lower bound.</param>
        /// <param name="hi">The upper bound.</param>
        /// <returns><c>true</c> if sorting was successful, <c>false</c> otherwise.</returns>
        private static bool TryComparableTimSort<TItem>(TItem[] array, int lo, int hi)
        {
            ArgumentNullException.ThrowIfNull(array);

            if (!IsIComparable<TItem>())
            {
                return false;
            }

            GetComparableSorterProxy<TItem[], TItem>(typeof(ComparableArrayTimSort<>)).SortRange(array, lo, hi);

            return true;
        }

        /// <summary>Tries sorting as IComparable.</summary>
        /// <typeparam name="TItem">The type of the item.</typeparam>
        /// <param name="list">The list.</param>
        /// <returns><c>true</c> if sorting was successful, <c>false</c> otherwise.</returns>
        private static bool TryComparableTimSort<TItem>(List<TItem> list)
        {
            ArgumentNullException.ThrowIfNull(list);

            if (!IsIComparable<TItem>())
            {
                return false;
            }

            GetComparableSorterProxy<TItem[], TItem>(typeof(ComparableListTimSort<>)).SortAll(list);

            return true;
        }

        /// <summary>Tries sorting as IComparable.</summary>
        /// <typeparam name="TItem">The type of the item.</typeparam>
        /// <param name="list">The list.</param>
        /// <param name="lo">The lower bound.</param>
        /// <param name="hi">The upper bound.</param>
        /// <returns><c>true</c> if sorting was successful, <c>false</c> otherwise.</returns>
        private static bool TryComparableTimSort<TItem>(List<TItem> list, int lo, int hi)
        {
            ArgumentNullException.ThrowIfNull(list);

            if (!IsIComparable<TItem>())
            {
                return false;
            }

            GetComparableSorterProxy<TItem[], TItem>(typeof(ComparableListTimSort<>)).SortRange(list, lo, hi);

            return true;
        }

        /// <summary>Tries sorting as IComparable.</summary>
        /// <typeparam name="TItem">The type of the item.</typeparam>
        /// <param name="list">The list.</param>
        /// <returns><c>true</c> if sorting was successful, <c>false</c> otherwise.</returns>
        private static bool TryComparableTimSort<TItem>(IList<TItem> list)
        {
            ArgumentNullException.ThrowIfNull(list);

            if (!IsIComparable<TItem>())
            {
                return false;
            }

            GetComparableSorterProxy<TItem[], TItem>(typeof(ComparableIListTimSort<>)).SortAll(list);

            return true;
        }

        /// <summary>Tries sorting as IComparable.</summary>
        /// <typeparam name="TItem">The type of the item.</typeparam>
        /// <param name="list">The list.</param>
        /// <param name="lo">The lower bound.</param>
        /// <param name="hi">The upper bound.</param>
        /// <returns><c>true</c> if sorting was successful, <c>false</c> otherwise.</returns>
        private static bool TryComparableTimSort<TItem>(IList<TItem> list, int lo, int hi)
        {
            ArgumentNullException.ThrowIfNull(list);

            if (!IsIComparable<TItem>())
            {
                return false;
            }

            GetComparableSorterProxy<TItem[], TItem>(typeof(ComparableIListTimSort<>)).SortRange(list, lo, hi);

            return true;
        }

        /// <summary>The map of '_items' member in <see cref="List{T}"/></summary>
        private static readonly Dictionary<Type, FieldInfo> ItemMemberMap =
            new Dictionary<Type, FieldInfo>();

        /// <summary>Tries to get '_items' member from <see cref="List{T}" />.</summary>
        /// <typeparam name="TItem">The type of the item.</typeparam>
        /// <param name="list">The list.</param>
        /// <returns>Array of items (if available)</returns>
        private static TItem[] GetInternalMember<TItem>(List<TItem> list)
        {
            ArgumentNullException.ThrowIfNull(list);

            var listType = typeof(List<TItem>);
            if (!ItemMemberMap.TryGetValue(listType, out var member))
            {
                member = typeof(List<TItem>).GetField("_items", BindingFlags.NonPublic | BindingFlags.Instance);

                if (member is not null)
                {
                    ItemMemberMap.Add(listType, member);
                }
            }

            if (member is null)
            {
                return Array.Empty<TItem>();
            }

            var memberValue = member.GetValue(list);
            if (memberValue is null)
            {
                return Array.Empty<TItem>();
            }

            return (TItem[])memberValue;
        }

        // ReSharper disable ParameterTypeCanBeEnumerable.Local

        /// <summary>Tries to cast <see cref="IList{T}" /> to <see cref="List{T}" />.</summary>
        /// <typeparam name="TItem">The type of the item.</typeparam>
        /// <param name="list">The list.</param>
        /// <returns><see cref="List{T}"/> if possible.</returns>
        private static List<TItem> GetInternalMember<TItem>(IList<TItem> list)
        {
            ArgumentNullException.ThrowIfNull(list);

            return (List<TItem>)list;
        }

        // ReSharper restore ParameterTypeCanBeEnumerable.Local
    }
}

// ReSharper restore CheckNamespace
