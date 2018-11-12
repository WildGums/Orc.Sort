// --------------------------------------------------------------------------------------------------------------------
// <copyright file="ComparableComparer.cs" company="WildGums">
//   Copyright (c) 2008 - 2018 WildGums. All rights reserved.
// </copyright>
// --------------------------------------------------------------------------------------------------------------------


namespace Orc.Sort.NSort
{
    using System;
    using System.Collections;

    /// <summary>
    ///     Default <see cref="IComparable" /> object comparer.
    /// </summary>
    public class ComparableComparer : IComparer
    {
        #region IComparer Members
        int IComparer.Compare(object x, object y)
        {
            return Compare((IComparable)x, y);
        }
        #endregion

        #region Methods
        public int Compare(IComparable x, object y)
        {
            return x.CompareTo(y);
        }
        #endregion
    }
}
