/*
Author: Milosz Krajewski
License: Apache License Version 2.0, January 2004 http://www.apache.org/licenses/ 
Description: TimSort extension methods.
*/

using System.Collections.Generic;
using System.Reflection;
using TimSort;

// ReSharper disable CheckNamespace

namespace System.Linq
{
	#region class TimSortExtender

    /// <summary>
    /// <![CDATA[T[], List<T> and IList<T>]]> extender providing TimSort extension methods.
    /// </summary>
	public static partial class TimSortExtender
    {
        #region dynamic invocation for IComparable

        #region class SorterProxy

        /// <summary>Proxy object to resolve sorter dynamically.</summary>
        internal class SorterProxy
        {
            #region fields (public)

            /// <summary>The sort all proxy.</summary>
            public Action<object> SortAll;

            /// <summary>The sort range proxy.</summary>
            public Action<object, int, int> SortRange;

            #endregion
        }

        #endregion

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
            var key = sorterType;
            SorterProxy sorter;

            if (!SorterProxyMap.TryGetValue(key, out sorter))
            {
                const BindingFlags flags = BindingFlags.Static | BindingFlags.NonPublic;
                sorter = new SorterProxy();
                var staticType = sorterType.MakeGenericType(typeof(TItem));
                var sortAll = (Action<TContainer>)Delegate.CreateDelegate(
                    typeof(Action<TContainer>),
                    staticType.GetMethod("SortAll", flags));
                var sortRange = (Action<TContainer, int, int>)Delegate.CreateDelegate(
                    typeof(Action<TContainer, int, int>),
                    staticType.GetMethod("SortRange", flags));
                sorter.SortAll = (array) => sortAll(array as TContainer);
                sorter.SortRange = (array, lo, hi) => sortRange(array as TContainer, lo, hi);
                SorterProxyMap[key] = sorter;
            }

            return sorter;
        }

        /// <summary>Tries sorting as IComparable.</summary>
        /// <typeparam name="TItem">Item type.</typeparam>
        /// <param name="array">The array.</param>
        /// <returns><c>true</c> if sorting was successful, <c>false</c> otherwise.</returns>
        private static bool TryComparableTimSort<TItem>(TItem[] array)
        {
            if (!IsIComparable<TItem>()) return false;
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
            if (!IsIComparable<TItem>()) return false;
            GetComparableSorterProxy<TItem[], TItem>(typeof(ComparableArrayTimSort<>)).SortRange(array, lo, hi);
            return true;
        }

        /// <summary>Tries sorting as IComparable.</summary>
        /// <typeparam name="TItem">The type of the item.</typeparam>
        /// <param name="list">The list.</param>
        /// <returns><c>true</c> if sorting was successful, <c>false</c> otherwise.</returns>
        private static bool TryComparableTimSort<TItem>(List<TItem> list)
        {
            if (!IsIComparable<TItem>()) return false;
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
            if (!IsIComparable<TItem>()) return false;
            GetComparableSorterProxy<TItem[], TItem>(typeof(ComparableListTimSort<>)).SortRange(list, lo, hi);
            return true;
        }

        /// <summary>Tries sorting as IComparable.</summary>
        /// <typeparam name="TItem">The type of the item.</typeparam>
        /// <param name="list">The list.</param>
        /// <returns><c>true</c> if sorting was successful, <c>false</c> otherwise.</returns>
        private static bool TryComparableTimSort<TItem>(IList<TItem> list)
        {
            if (!IsIComparable<TItem>()) return false;
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
            if (!IsIComparable<TItem>()) return false;
            GetComparableSorterProxy<TItem[], TItem>(typeof(ComparableIListTimSort<>)).SortRange(list, lo, hi);
            return true;
        }

        #endregion

        #region internal implementation access

        /// <summary>The map of '_items' member in <see cref="List{T}"/></summary>
	    private static readonly Dictionary<Type, FieldInfo> ItemMemberMap = 
            new Dictionary<Type, FieldInfo>();

        /// <summary>Tries to get '_items' member from <see cref="List{T}" />.</summary>
        /// <typeparam name="TItem">The type of the item.</typeparam>
        /// <param name="list">The list.</param>
        /// <returns>Array of items (if available)</returns>
        private static TItem[] GetInternalMember<TItem>(List<TItem> list)
        {
            FieldInfo member;
            var listType = typeof (List<TItem>);
            if (!ItemMemberMap.TryGetValue(listType, out member))
            {
                member = typeof (List<TItem>).GetField("_items", BindingFlags.NonPublic | BindingFlags.Instance);
                ItemMemberMap.Add(listType, member);
            }
            if (member == null) return null;
            return (TItem[])member.GetValue(list);
        }

        // ReSharper disable ParameterTypeCanBeEnumerable.Local

        /// <summary>Tries to cast <see cref="IList{T}" /> to <see cref="List{T}" />.</summary>
        /// <typeparam name="TItem">The type of the item.</typeparam>
        /// <param name="list">The list.</param>
        /// <returns><see cref="List{T}"/> if possible.</returns>
        private static List<TItem> GetInternalMember<TItem>(IList<TItem> list)
        {
            return list as List<TItem>;
        }

        // ReSharper restore ParameterTypeCanBeEnumerable.Local

        #endregion
    }

	#endregion
}

// ReSharper restore CheckNamespace
