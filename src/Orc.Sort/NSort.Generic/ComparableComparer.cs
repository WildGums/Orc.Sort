namespace Orc.Sort.NSort.Generic
{
    using System;
    using System.Collections.Generic;

    /// <summary>
    /// Default <see cref="IComparable"/> object comparer.
    /// </summary>
    public class ComparableComparer<T> : IComparer<T>
    {
        int IComparer<T>.Compare(T? x, T? y)
        {
            return Compare((IComparable<T>?) x, y);
        }

        public int Compare(IComparable<T>? x, T? y)
        {
            if (x is null && y is null)
            {
                return 0;
            }

            if (x is null)
            {
                return -1;
            }

            if (y is null)
            {
                return 1;
            }

            return x.CompareTo(y);
        }
    }
}
