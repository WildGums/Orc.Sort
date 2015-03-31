// --------------------------------------------------------------------------------------------------------------------
// <copyright file="DefaultSwap.cs" company="Orcomp development team">
//   Copyright (c) 2008 - 2015 Orcomp development team. All rights reserved.
// </copyright>
// --------------------------------------------------------------------------------------------------------------------


namespace Orc.Sort.NSort.Generic
{
    using System.Collections.Generic;
    

    /// <summary>
    /// Default swap class
    /// </summary>
    public class DefaultSwap<T> : ISwap<T>
    {
        #region Methods
        public void Swap(IList<T> array, int left, int right)
        {
            T swap = array[left];
            array[left] = array[right];
            array[right] = swap;
        }

        public void Set(IList<T> array, int left, int right)
        {
            array[left] = array[right];
        }

        public void Set(IList<T> array, int left, T obj)
        {
            array[left] = obj;
        }
        #endregion
    }
}