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
        ///     Sorts the elements of a sequence based on the order of the items in the specified template collection, using the
        ///     specified System.Collections.Generic.IEqualityComparer&lt;T&gt;.
        /// </summary>
        /// <example>
        ///     collection = [A, B, C, B, A, C, D, D]
        ///     template = [B, A, D, C]
        ///     collection.SortAccordingTo(template) = [B, B, A, A, D, D, C, C]
        /// </example>
        /// <typeparam name="T">
        ///     The type of the collection to be sorted.
        /// </typeparam>
        /// <param name="sourceCollection">
        ///     The sequence to be sorted.
        /// </param>
        /// <param name="templateCollection">
        ///     A template collection that specifies the order in which the sequence will be sorted.
        ///     May not contain duplicates.
        /// </param>
        /// <param name="includeNoMatches">
        ///     Indicates whether the items in the source collection that do not match any items in the template collection should be appended to the result or not.
        /// </param>
        /// <param name="comparer">
        ///     A System.Collections.Generic.IEqualityComparer&lt;T&gt; implementation to use when comparing the
        ///     elements in the template collection, or null to use the default one.
        /// </param>
        /// <returns>
        ///     Returns a list containing the sorted items.
        /// </returns>
        /// <exception cref="ArgumentNullException">
        ///     Thrown when either sourceCollection, keySelector or templateCollection is null.
        /// </exception>
        /// <exception cref="ArgumentException">
        ///     Thrown when the template collection contains duplicates.
        /// </exception>
        public static IEnumerable<T> SortAccordingTo<T>(this IEnumerable<T> sourceCollection, IEnumerable<T> templateCollection, 
            bool includeNoMatches = true, IEqualityComparer<T>? comparer = null)
            where T : notnull
        {
            return SortAccordingTo(sourceCollection, templateCollection, x => x, includeNoMatches, comparer);
        }

        /// <summary>
        ///     Sorts the elements of a sequence based on the order of the items in the specified template collection, using the
        ///     specified System.Collections.Generic.IEqualityComparer&lt;T&gt;.
        /// </summary>
        /// <example>
        ///     collection = [A, B, C, B, A, C, D, D]
        ///     template = [B, A, D, C]
        ///     collection.SortAccordingTo(template) = [B, B, A, A, D, D, C, C]
        /// </example>
        /// <typeparam name="TSource">
        ///     The type of the items to be sorted.
        /// </typeparam>
        /// <typeparam name="TKey">
        ///     The type of the items in the template collection.
        /// </typeparam>
        /// <param name="sourceCollection">
        ///     The sequence to be sorted.
        /// </param>
        /// <param name="templateCollection">
        ///     A template collection that specifies the order in which the sequence will be sorted.
        ///     May not contain duplicates.
        /// </param>
        /// <param name="keySelector">
        ///     A function to extract a key from an element.
        /// </param>
        /// <param name="includeNoMatches">
        ///     Indicates whether the items in the source collection that do not match any items in the template collection should be appended to the result or not.
        /// </param>
        /// <param name="comparer">
        ///     A System.Collections.Generic.IEqualityComparer&lt;T&gt; implementation to use when comparing the
        ///     elements in the template collection, or null to use the default one.
        /// </param>
        /// <returns>
        ///     Returns a list containing the sorted items.
        /// </returns>
        /// <exception cref="ArgumentNullException">
        ///     Thrown when either sourceCollection, keySelector or templateCollection is null.
        /// </exception>
        /// <exception cref="ArgumentException">
        ///     Thrown when the template collection contains duplicates.
        /// </exception>
        public static IEnumerable<TSource> SortAccordingTo<TSource, TKey>(this IEnumerable<TSource> sourceCollection, 
            IEnumerable<TKey> templateCollection, Func<TSource, TKey> keySelector, bool includeNoMatches = true, 
            IEqualityComparer<TKey>? comparer = null)
            where TSource : notnull
            where TKey : notnull
        {
            if (sourceCollection is null)
            {
                throw new ArgumentNullException("sourceCollection cannot be null.");
            }

            if (keySelector is null)
            {
                throw new ArgumentNullException("keySelector cannot be null.");
            }

            if (templateCollection is null)
            {
                throw new ArgumentNullException("templateCollection cannot be null.");
            }

            // Stores items that do not have a match in the template collection
            var noMatches = new Dictionary<TSource, List<TSource>>();

            var buckets = new Dictionary<TKey, List<TSource>>(comparer);

            // Build buckets based on the values in the template collection.
            foreach (var item in templateCollection)
            {
                if (!buckets.ContainsKey(item))
                {
                    buckets.Add(item, new List<TSource>());
                }
                else
                {
                    throw new ArgumentNullException("templateCollection cannot have duplicates.");
                }
            }

            // Insert values from source collection into correct buckets.
            foreach (var item in sourceCollection)
            {
                var key = keySelector(item);

                if (buckets.ContainsKey(key))
                {
                    buckets[key].Add(item);
                }
                else if (includeNoMatches)
                {
                    if (noMatches.ContainsKey(item))
                    {
                        noMatches[item].Add(item);
                    }
                    else
                    {
                        noMatches[item] = new List<TSource>{item};
                    }
                }
            }

            // Build the result
            var result = new List<TSource>();

            foreach (var key in templateCollection)
            {
                result.AddRange(buckets[key]);
            }

            if (includeNoMatches)
            {
                result.AddRange(noMatches.SelectMany(x => x.Value));
            }

            return result;
        }
        #endregion
    }
}
