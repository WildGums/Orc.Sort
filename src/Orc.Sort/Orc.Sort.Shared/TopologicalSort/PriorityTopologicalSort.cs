// --------------------------------------------------------------------------------------------------------------------
// <copyright file="PriorityTopologicalSort.cs" company="Orcomp development team">
//   Copyright (c) 2008 - 2015 Orcomp development team. All rights reserved.
// </copyright>
// --------------------------------------------------------------------------------------------------------------------


namespace Orc.Sort.TopologicalSort
{
    using System;

    /// <summary>
    /// Uses a standard topological sort but gives priority to an item if it is encountered first.
    /// </summary>
    /// <typeparam name="T">
    /// </typeparam>
    public class PriorityTopologicalSort<T> : TopologicalSort<T>
        where T : IEquatable<T>
    {
        #region Constructors
        /// <summary>
        /// Initializes a new instance of the <see cref="PriorityTopologicalSort{T}"/> class.
        /// </summary>
        public PriorityTopologicalSort()
        {
            base.UsesPriority = true;
        }
        #endregion
    }
}