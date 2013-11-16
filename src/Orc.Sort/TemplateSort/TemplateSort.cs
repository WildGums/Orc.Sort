// --------------------------------------------------------------------------------------------------------------------
// <copyright file="TemplateSort.cs" company="">
//   
// </copyright>
// <summary>
//   The template sort 2.
// </summary>
// --------------------------------------------------------------------------------------------------------------------

namespace Orc.Sort.TemplateSort
{
    using System;
    using System.Collections.Generic;
    using System.Linq;

    /// <summary>
    /// The template sort.
    /// </summary>
    public static class TemplateSort
    {
        #region Methods

        /// <summary>
        /// Sorts the elements of a sequence based on the order of the items in the specified template collection, using the
        ///     specified System.Collections.Generic.IEqualityComparer&lt;T&gt;.
        /// </summary>
        /// <example>
        /// collection = [A, B, C, B, A, C, D, D]
        ///     template = [B, A, D, C]
        ///     collection.SortAccordingTo(template) = [B, B, A, A, D, D, C, C]
        /// </example>
        /// <typeparam name="T">
        /// The type of the collection to be sorted.
        /// </typeparam>
        /// <param name="sourceCollection">
        /// The sequence to be sorted.
        /// </param>
        /// <param name="templateCollection">
        /// A template collection that specifies the order in which the sequence will be sorted.
        ///     May not contain duplicates.
        /// </param>
        /// <param name="comparer">
        /// A System.Collections.Generic.IEqualityComparer&lt;T&gt; implementation to use when comparing the
        ///     elements in the template collection, or null to use the default one.
        /// </param>
        /// <returns>
        /// Returns a list containing the sorted items.
        /// </returns>
        /// <exception cref="ArgumentNullException">
        /// Thrown when either sourceCollection, keySelector or templateCollection is null.
        /// </exception>
        /// <exception cref="ArgumentException">
        /// Thrown when the template collection contains duplicates.
        /// </exception>
        public static IEnumerable<T> SortAccordingTo<T>(this IEnumerable<T> sourceCollection, IEnumerable<T> templateCollection, IEqualityComparer<T> comparer = null)
        {
            return SortAccordingTo(sourceCollection, templateCollection, x => x, comparer);
        }

        /// <summary>
        /// Sorts the elements of a sequence based on the order of the items in the specified template collection, using the
        ///     specified System.Collections.Generic.IEqualityComparer&lt;T&gt;.
        /// </summary>
        /// <example>
        /// collection = [A, B, C, B, A, C, D, D]
        ///     template = [B, A, D, C]
        ///     collection.SortAccordingTo(template) = [B, B, A, A, D, D, C, C]
        /// </example>
        /// <typeparam name="TSource">
        /// The type of the items to be sorted.
        /// </typeparam>
        /// <typeparam name="TKey">
        /// The type of the items in the template collection.
        /// </typeparam>
        /// <param name="sourceCollection">
        /// The sequence to be sorted.
        /// </param>
        /// <param name="templateCollection">
        /// A template collection that specifies the order in which the sequence will be sorted.
        ///     May not contain duplicates.
        /// </param>
        /// <param name="keySelector">
        /// A function to extract a key from an element.
        /// </param>
        /// <param name="comparer">
        /// A System.Collections.Generic.IEqualityComparer&lt;T&gt; implementation to use when comparing the
        ///     elements in the template collection, or null to use the default one.
        /// </param>
        /// <returns>
        /// Returns a list containing the sorted items.
        /// </returns>
        /// <exception cref="ArgumentNullException">
        /// Thrown when either sourceCollection, keySelector or templateCollection is null.
        /// </exception>
        /// <exception cref="ArgumentException">
        /// Thrown when the template collection contains duplicates.
        /// </exception>
        public static IEnumerable<TSource> SortAccordingTo<TSource, TKey>(this IEnumerable<TSource> sourceCollection, IEnumerable<TKey> templateCollection, Func<TSource, TKey> keySelector, IEqualityComparer<TKey> comparer = null)
        {
            if (sourceCollection == null)
            {
                throw new ArgumentNullException("sourceCollection cannot be null.");
            }

            if (keySelector == null)
            {
                throw new ArgumentNullException("keySelector cannot be null.");
            }

            if (templateCollection == null)
            {
                throw new ArgumentNullException("templateCollection cannot be null.");
            }

            // Stores items that do not have a match in the template collection
            var noMatches = new List<TSource>();

            var buckets = new Dictionary<TKey, List<TSource>>(comparer);

            // Build buckets based on the values in the template collection.
            foreach (TKey item in templateCollection)
            {
                buckets.Add(item, new List<TSource>());
            }

            // Insert values from source collection into correct buckets.
            foreach (TSource item in sourceCollection)
            {
                TKey key = keySelector(item);

                if (buckets.ContainsKey(key))
                {
                    buckets[key].Add(item);
                }
                else
                {
                    noMatches.Add(item);
                }
            }

            // Build the result
            var result = new List<TSource>();

            foreach (TKey key in templateCollection)
            {
                result.AddRange(buckets[key]);
            }

            result.AddRange(noMatches);

            return result;
        }

        #endregion
    }
}