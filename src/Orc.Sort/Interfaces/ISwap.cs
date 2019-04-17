// --------------------------------------------------------------------------------------------------------------------
// <copyright file="ISwap.cs" company="WildGums">
//   Copyright (c) 2008 - 2018 WildGums. All rights reserved.
// </copyright>
// --------------------------------------------------------------------------------------------------------------------

namespace Orc.Sort
{
    using System.Collections;
    using System.Collections.Generic;

    /// <summary>
    /// </summary>
    /// Object swapper interface
    public interface ISwap
    {
        #region Methods
        void Swap(IList array, int left, int right);
        void Set(IList array, int left, int right);
        void Set(IList array, int left, object obj);
        #endregion
    }

    public interface ISwap<T>
    {
        #region Methods
        void Swap(IList<T> array, int left, int right);
        void Set(IList<T> array, int left, int right);
        void Set(IList<T> array, int left, T obj);
        #endregion
    }
}
