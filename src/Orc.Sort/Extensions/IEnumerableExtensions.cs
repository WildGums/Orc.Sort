using System;
using System.Collections;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace Orc.Sort.Extensions
{
    public static class IEnumerableExtensions
    {
        public static IEnumerable<T> MergeSorted<T>(this IEnumerable<IEnumerable<T>> sortedEnumerables)
            where T : IComparable<T>
        {
            var set = new SortedSet<KeyedEnumerator<T>>();
            int key = 0;

            foreach (var enumerator in sortedEnumerables.Select(e => e.GetEnumerator()))
            {
                if (enumerator.MoveNext())
                    set.Add(new KeyedEnumerator<T>(enumerator, key++));
            }

            while (set.Count > 1)
            {
                var min = set.Min;
                var has = true;
                set.Remove(min);

                while (has && min.CompareTo(set.Min) < 0)
                {
                    yield return min.Current;
                    has = min.MoveNext();
                }

                if (has)
                {
                    set.Add(min);
                }
            }

            if (set.Count > 0)
            {
                var min = set.Min;
                var has = true;
                set.Remove(min);

                while (has)
                {
                    yield return min.Current;
                    has = min.MoveNext();
                }
            }

        }
    }

    internal class EnumeratorComparer<T> : IComparer<IEnumerator<T>>
        where T : IComparable<T>
    {
        public int Compare(IEnumerator<T> x, IEnumerator<T> y)
        {
            var result = x.Current.CompareTo(y.Current);
            if (result == 0)
                result = x.GetHashCode() - y.GetHashCode();

            return result;
        }
    }

    internal class KeyedEnumerator<T> : IEnumerator<T>, IComparable<KeyedEnumerator<T>>
        where T : IComparable<T>
    {
        public KeyedEnumerator(IEnumerator<T> enumerator, int key)
        {
            Enumerator = enumerator;
            Key = key;
        }

        public IEnumerator<T> Enumerator { get; private set; }
        public int Key { get; private set; }

        public int CompareTo(KeyedEnumerator<T> other)
        {
            var result = Current.CompareTo(other.Current);
            if (result == 0)
                result = Key.CompareTo(other.Key);

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

        public T Current
        {
            get { return Enumerator.Current; }
        }

        object IEnumerator.Current
        {
            get { return Enumerator.Current; }
        }
    }
}
