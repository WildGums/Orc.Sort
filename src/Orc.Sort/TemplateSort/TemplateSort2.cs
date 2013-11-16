namespace Orc.Sort.TemplateSort
{
    using System;
    using System.Collections.Generic;

    public static class TemplateSort2
    {
        /// <summary>
        /// Sorts the elements of a sequence based on the order of the items in the specified template collection.
        /// </summary>
        /// <typeparam name="TKey">The type of the items in the template collection.</typeparam>
        /// <typeparam name="TSource">The type of the items to be sorted.</typeparam>
        /// <param name="sourceCollection">The sequence to be sorted.</param>
        /// <param name="keySelector">A function to extract a key from an element.</param>
        /// <param name="templateCollection">A template collection that specifies the order in which the sequence will be sorted. May not contain duplicates.</param>
        /// <returns>Returns a list containing the sorted items.</returns>
        /// <exception cref="ArgumentNullException">Thrown when either sourceCollection, keySelector or templateCollection is null.</exception>
        /// <exception cref="ArgumentException">Thrown when the template collection contains duplicates.</exception>
        public static SingleLinkedList<TSource> OrderBy2<TSource, TKey>(this IEnumerable<TSource> sourceCollection, Func<TSource, TKey> keySelector, IEnumerable<TKey> templateCollection)
        {
            return OrderBy2(sourceCollection, keySelector, templateCollection, null);
        }

        /// <summary>
        /// Sorts the elements of a sequence based on the order of the items in the specified template collection, using the specified System.Collections.Generic.IEqualityComparer&lt;T&gt;.
        /// </summary>
        /// <typeparam name="TKey">The type of the items in the template collection.</typeparam>
        /// <typeparam name="TSource">The type of the items to be sorted.</typeparam>
        /// <param name="sourceCollection">The sequence to be sorted.</param>
        /// <param name="keySelector">A function to extract a key from an element.</param>
        /// <param name="templateCollection">A template collection that specifies the order in which the sequence will be sorted. May not contain duplicates.</param>
        /// <param name="comparer">A System.Collections.Generic.IEqualityComparer&lt;T&gt; implementation to use when comparing the elements in the template collection, or null to use the default one.</param>
        /// <returns>Returns a list containing the sorted items.</returns>
        /// <exception cref="ArgumentNullException">Thrown when either sourceCollection, keySelector or templateCollection is null.</exception>
        /// <exception cref="ArgumentException">Thrown when the template collection contains duplicates.</exception>
        public static SingleLinkedList<TSource> OrderBy2<TSource, TKey>(this IEnumerable<TSource> sourceCollection, Func<TSource, TKey> keySelector, IEnumerable<TKey> templateCollection, IEqualityComparer<TKey> comparer = null)
        {
            if (sourceCollection == null)
                throw new ArgumentNullException("sourceCollection");
            if (keySelector == null)
                throw new ArgumentNullException("keySelector");
            if (templateCollection == null)
                throw new ArgumentNullException("templateCollection");

            SingleLinkedList<TSource> notMatching = new SingleLinkedList<TSource>();
            Dictionary<TKey, SingleLinkedList<TSource>> dictionary = new Dictionary<TKey, SingleLinkedList<TSource>>(comparer);
            foreach (TKey key in templateCollection)
                dictionary.Add(key, new SingleLinkedList<TSource>());
            
            foreach (TSource item in sourceCollection)
            {
                TKey key = keySelector(item);
                if (dictionary.ContainsKey(key))
                    dictionary[key].Add(item);
                else
                    notMatching.Add(item);
            }
            
            SingleLinkedList<TSource> result = new SingleLinkedList<TSource>();
            foreach (TKey key in templateCollection)
                result.Concat(dictionary[key]);
            result.Concat(notMatching);
            return result;
        }
    }
}
