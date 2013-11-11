// --------------------------------------------------------------------------------------------------------------------
// <copyright file="TemplateSort.cs" company="">
//   
// </copyright>
// <summary>
//   Contains extension methods for the ICollection interface that allow to sort the given collection
//   according to some other collection used as template.
// </summary>
// --------------------------------------------------------------------------------------------------------------------

namespace Orc.Sort.TemplateSort
{
    using System;
    using System.Collections.Generic;
    using System.Linq;

    /// <summary>
    ///     Contains extension methods for the ICollection interface that allow to sort the given collection
    ///     according to some other collection used as template.
    /// </summary>
    /// <remarks>
    ///     In order to make this methods available for an ICollection object, just import this class wherever you want to use
    ///     them.
    /// </remarks>
    public static class TemplateSort
    {
        #region Methods

        /// <summary>
        /// Sorts the collection according to the given template collection.
        /// </summary>
        /// <example>
        ///     collection = [A, B, C, B, A, C, D, D]
        ///     template = [B, A, D, C]
        ///     collection.SortAccordingTo(template) = [B, B, A, A, D, D, C, C]
        /// </example>
        /// <typeparam name="T">
        /// The type of the elements of the collection
        /// </typeparam>
        /// <param name="collection">
        /// The collection to be sorted
        /// </param>
        /// <param name="template">
        /// The template collection to sort according to it. Must not have duplicates.
        /// </param>
        /// <remarks>
        /// As long as this is an extension method, collection won't be passed as a parameter, but you can invoke this
        ///     method on it.
        /// </remarks>
        /// <returns>
        /// A collection result of ordering the original collection according to the order of the elements of the template
        ///     collection.
        /// </returns>
        public static ICollection<T> SortAccordingTo<T>(this ICollection<T> collection, ICollection<T> template)
        {
            if ((collection.Count == 0) || template.Count == 0)
            {
                return collection;
            }

            var result = new List<T>();
            var other = new List<T>();

            // Initialize a hash table, mapping each element of the template with a counter.
            Dictionary<T, int> hash;
            try
            {
                hash = template.ToDictionary(t => t, t => 0);
            }
            catch (ArgumentException e)
            {
                throw new ArgumentException("Template collection must not have duplicated elements", e);
            }

            // Count how many of each element of the template are there in the collection.
            foreach (var element in collection)
            {
                int val;
                if (hash.TryGetValue(element, out val))
                {
                    hash[element] = ++val;
                }
                else
                {
                    other.Add(element); // If the element is not in the template, add it to the others list.
                }
            }

            // For each element of the template (in order), add to the result as many elements as equal elements are there in the collection.
            foreach (var k in template)
            {
                for (var i = 0; i < hash[k]; i++)
                {
                    result.Add(k);
                }
            }

            // Add the elements that don't match with any elements of the template.
            result.AddRange(other);
            return result;
        }

        /// <summary>
        /// Sorts the collection, using a certain property of its elements, according to the given template collection.
        /// </summary>
        /// <example>
        /// Suppose collection elements are objects with two properties V1 and V2.
        ///     collection = [{V1 = A, V2 = 2}, {V1 = B, V2 = 1}, {V1 = C, V2 = 2}]
        ///     template = [1, 2]
        ///     collection.SortAccordingTo(x =&gt; x.V2, template) = [{V1 = B, V2 = 1}, {V1 = A, V2 = 2}, {V1 = C, V2 = 2}]
        /// </example>
        /// <typeparam name="T">
        /// The type of the elements of the collection to order
        /// </typeparam>
        /// <typeparam name="T2">
        /// The type of the elements of the property that will be used for ordering according to the template collection.
        ///     Note that the elements of the template collection must have the same type.
        /// </typeparam>
        /// <param name="collection">
        /// The collection to order.
        /// </param>
        /// <param name="orderingProperty">
        /// A lambda expression that gets some property of the elements of the collection.
        /// </param>
        /// <param name="template">
        /// The template collection to sort according to it. Must not have duplicates.
        /// </param>
        /// <remarks>
        /// As long as this is an extension method, collection won't be passed as a parameter, but you can invoke this
        ///     method on it.
        /// </remarks>
        /// <returns>
        /// A collection result of ordering the original collection, taking the values of the selected property of its
        ///     elements, according to the order of the elements of the template collection.
        /// </returns>
        public static ICollection<T> SortAccordingTo<T, T2>(this ICollection<T> collection, Func<T, T2> orderingProperty, ICollection<T2> template)
        {
            if ((collection.Count == 0) || template.Count == 0)
            {
                return collection;
            }

            var result = new List<T>();
            var other = new List<T>();

            // Initialize a hash that maps each element in the template with a new list
            // that is going to contain the elements of the original collection whose ordering property equals the mapped template element.
            Dictionary<T2, List<T>> hash;
            try
            {
                hash = template.ToDictionary(t => t, t => new List<T>());
            }
            catch (ArgumentException e)
            {
                throw new ArgumentException("Template collection must not have duplicated elements", e);
            }

            // For each element of the collection, add it to the list that corresponds to its ordering property value.
            foreach (var element in collection)
            {
                List<T> val;
                if (hash.TryGetValue(orderingProperty(element), out val))
                {
                    val.Add(element);
                }
                else
                {
                    other.Add(element); // If the element is not in the template, add it to the others list.
                }
            }

            // Add the mapped lists to the result list in the order given by the template collection.
            foreach (var k in template)
            {
                result.AddRange(hash[k]);
            }

            // Add the elements (in order) that don't match with any elements of the template.
            result.AddRange(other);
            return result;
        }

        #endregion
    }
}