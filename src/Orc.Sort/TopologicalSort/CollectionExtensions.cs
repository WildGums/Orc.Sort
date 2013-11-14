namespace Orc.Sort.TopologicalSort
{
    using System.Collections.Generic;
    using System.Collections.ObjectModel;
    using System.Linq;

    public static class CollectionExtensions
    {
        public static ReadOnlyCollection<T> AsReadOnly<T>(this IList<T> list)
        {
            return new ReadOnlyCollection<T>(list);
        }

        public static IEnumerable<T> ConcatWith<T>(this IEnumerable<T> collection, T item)
        {
            return collection.Concat(new[] { item });
        }
    }
}