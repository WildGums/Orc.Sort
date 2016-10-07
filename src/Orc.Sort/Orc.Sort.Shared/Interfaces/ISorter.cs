// --------------------------------------------------------------------------------------------------------------------
// <copyright file="ISorter.cs" company="WildGums">
//   Copyright (c) 2008 - 2015 WildGums. All rights reserved.
// </copyright>
// --------------------------------------------------------------------------------------------------------------------


namespace Orc.Sort
{
    using System.Collections;
    using System.Collections.Generic;

    /// <summary>
    /// Summary description for ISorter.
    /// </summary>
    public interface ISorter
    {
        #region Methods
        void Sort(IList list);
        #endregion
    }

    public interface ISorter<T>
    {
        #region Methods
        void Sort(IList<T> list);
        #endregion
    }
}