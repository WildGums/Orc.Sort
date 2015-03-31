// --------------------------------------------------------------------------------------------------------------------
// <copyright file="DefaultSwap.cs" company="Orcomp development team">
//   Copyright (c) 2008 - 2015 Orcomp development team. All rights reserved.
// </copyright>
// --------------------------------------------------------------------------------------------------------------------


namespace Orc.Sort.NSort
{
    using System.Collections;
    

    /// <summary>
    /// Default swap class
    /// </summary>
    public class DefaultSwap : ISwap
    {
        #region Methods
        public void Swap(IList array, int left, int right)
        {
            object swap = array[left];
            array[left] = array[right];
            array[right] = swap;
        }

        public void Set(IList array, int left, int right)
        {
            array[left] = array[right];
        }

        public void Set(IList array, int left, object obj)
        {
            array[left] = obj;
        }
        #endregion
    }
}