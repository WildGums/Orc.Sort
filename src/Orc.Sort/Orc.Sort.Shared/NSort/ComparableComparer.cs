// --------------------------------------------------------------------------------------------------------------------
// <copyright file="ComparableComparer.cs" company="Orcomp development team">
//   Copyright (c) 2008 - 2015 Orcomp development team. All rights reserved.
// </copyright>
// --------------------------------------------------------------------------------------------------------------------


namespace Orc.Sort.NSort
{
    using System;
    using System.Collections;

    /// <summary>
    /// Default <see cref="IComparable"/> object comparer.
    /// </summary>
    public class ComparableComparer : IComparer
    {
        #region Methods

        #region IComparer Members
        int IComparer.Compare(Object x, Object y)
        {
            return this.Compare((IComparable) x, y);
        }
        #endregion

        public int Compare(IComparable x, Object y)
        {
            return x.CompareTo(y);
        }
        #endregion
    }
}