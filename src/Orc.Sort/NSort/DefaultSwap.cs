// --------------------------------------------------------------------------------------------------------------------
// <copyright file="DefaultSwap.cs" company="WildGums">
//   Copyright (c) 2008 - 2018 WildGums. All rights reserved.
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
        #region ISwap Members
        public void Swap(IList array, int left, int right)
        {
            var swap = array[left];
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
