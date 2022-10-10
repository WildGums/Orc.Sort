namespace Orc.Sort.NSort
{
    using System;
    using System.Collections;

    /// <summary>
    ///     Default <see cref="IComparable" /> object comparer.
    /// </summary>
    public class ComparableComparer : IComparer
    {
        int IComparer.Compare(object? x, object? y)
        {
            return Compare((IComparable?)x, y);
        }
        
        public int Compare(IComparable? x, object? y)
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
