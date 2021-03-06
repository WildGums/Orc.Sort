// --------------------------------------------------------------------------------------------------------------------
// <copyright file="ComparableComparer.cs" company="WildGums">
//   Copyright (c) 2008 - 2017 WildGums. All rights reserved.
// </copyright>
// --------------------------------------------------------------------------------------------------------------------


namespace Orc.Sort.NSort.Generic
{
    using System;
    using System.Collections.Generic;

    /// <summary>
    /// Default <see cref="IComparable"/> object comparer.
    /// </summary>
    public class ComparableComparer<T> : IComparer<T>
    {
        #region Methods

        #region IComparer Members
        int IComparer<T>.Compare(T x, T y)
        {
            return this.Compare((IComparable<T>) x, y);
        }
        #endregion

        public int Compare(IComparable<T> x, T y)
        {
            return x.CompareTo(y);
        }
        #endregion
    }
}