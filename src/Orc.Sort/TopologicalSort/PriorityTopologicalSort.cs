// --------------------------------------------------------------------------------------------------------------------
// <copyright file="PriorityTopologicalSort.cs" company="orc">
//   
// </copyright>
// <summary>
//   Uses a standard topological sort but gives priority to an item if it is encountered first.
// </summary>
// --------------------------------------------------------------------------------------------------------------------

namespace Orc.Sort.TopologicalSort
{
    using System;
    using System.Collections.Generic;
    using System.Linq;

    /// <summary>
    /// Uses a standard topological sort but gives priority to an item if it is encountered first.
    /// </summary>
    /// <typeparam name="T">
    /// </typeparam>
    public class PriorityTopologicalSort<T>
        where T : IEquatable<T>
    {
        #region Fields

        /// <summary>
        /// The edges from.
        /// </summary>
        private List<SortedSet<int>> edgesFrom;

        /// <summary>
        /// The edges into.
        /// </summary>
        private List<SortedSet<int>> edgesInto;

        /// <summary>
        /// The nodes dictionary.
        /// </summary>
        private Dictionary<T, int> nodesDict;

        /// <summary>
        /// The nodes list.
        /// </summary>
        private List<T> nodesList;

        /// <summary>
        /// The nodes next.
        /// </summary>
        private SortedSet<int> nodesNext;

        /// <summary>
        /// The nodes path.
        /// </summary>
        private List<T> nodesPath;

        #endregion

        #region Constructors

        /// <summary>
        /// Initializes a new instance of the <see cref="PriorityTopologicalSort{T}"/> class.
        /// </summary>
        public PriorityTopologicalSort()
        {
            SubLists = new List<IList<T>>();
        }

        #endregion

        #region Properties

        /// <summary>
        ///     Gets or sets the sub lists.
        ///     The order of the items in the sub list is important and will preserved in the final merged list
        /// </summary>
        public List<IList<T>> SubLists { get; set; }

        #endregion

        #region Methods

        /// <summary>
        /// The can sort.
        /// </summary>
        /// <param name="subList">
        /// The sub list.
        /// </param>
        /// <returns>
        /// The <see cref="bool"/>.
        /// </returns>
        public bool CanSort(IList<T> subList)
        {
            if (subList == null)
            {
                return false;
            }

            var topSort = new PriorityTopologicalSort<T>();
            topSort.SubLists = new List<IList<T>>(SubLists);
            topSort.SubLists.Add(subList);

            var result = topSort.Sort();

            return result != null;
        }

        /// <summary>
        /// The sort.
        /// </summary>
        /// <returns>
        /// The <see cref="List"/>.
        /// </returns>
        public List<T> Sort()
        {
            Initialize();
            Walk();

            if (nodesPath.Count == nodesList.Count)
            {
                return nodesPath;
            }

            return null;
        }

        /// <summary>
        /// Return the collection of lists that are in conflict with each other.
        /// </summary>
        /// <returns>
        /// The <see cref="List"/>.
        /// </returns>
        public List<IList<T>> GetConflicts()
        {
            var conflictLists = new List<IList<T>>();

            for (var i = 1; i < SubLists.Count; i++)
            {
                var topSort = new PriorityTopologicalSort<T>();
                topSort.SubLists = SubLists.Take(i).ToList();

                var sortedList = topSort.Sort();

                if (sortedList != null)
                {
                    continue;
                }

                var subList = SubLists.Take(i).Reverse().ToList();

                for (var j = 1; j < subList.Count; j++)
                {
                    topSort = new PriorityTopologicalSort<T>();
                    topSort.SubLists = SubLists.Take(j).ToList();

                    sortedList = topSort.Sort();

                    if (sortedList != null)
                    {
                        continue;
                    }

                    conflictLists.Add(subList[j - 1].ToList());
                    conflictLists.Add(subList.First().ToList());
                    break;
                }

                break;
            }

            return conflictLists;
        }

        /// <summary>
        /// The initialize.
        /// </summary>
        private void Initialize()
        {
            nodesDict = new Dictionary<T, int>();
            nodesList = new List<T>();
            nodesPath = new List<T>();

            nodesNext = new SortedSet<int>();

            edgesFrom = new List<SortedSet<int>>();
            edgesInto = new List<SortedSet<int>>();

            foreach (var yarn in SubLists)
            {
                int next = 0;
                int prev = 0;

                for (int i = 0; i < yarn.Count; i++)
                {
                    T node = yarn[i];

                    if (!nodesDict.ContainsKey(node))
                    {
                        nodesDict.Add(node, nodesDict.Count);
                        nodesList.Add(node);
                        edgesFrom.Add(new SortedSet<int>());
                        edgesInto.Add(new SortedSet<int>());
                    }

                    next = nodesDict[node];

                    if (i > 0)
                    {
                        edgesFrom[next].Add(prev);
                        edgesInto[prev].Add(next);
                    }

                    prev = next;
                }
            }
        }

        /// <summary>
        /// The walk.
        /// </summary>
        private void Walk()
        {
            nodesNext.UnionWith(Enumerable.Range(0, nodesDict.Count).Where(node => edgesInto[node].Count == 0));

            while (nodesNext.Count > 0)
            {
                int next = nodesNext.Max;
                nodesPath.Add(nodesList[next]);
                nodesNext.Remove(next);

                foreach (int node in edgesFrom[next])
                {
                    edgesInto[node].Remove(next);

                    if (edgesInto[node].Count == 0)
                    {
                        nodesNext.Add(node);
                    }
                }
            }

            nodesPath.Reverse();
        }

        #endregion
    }
}