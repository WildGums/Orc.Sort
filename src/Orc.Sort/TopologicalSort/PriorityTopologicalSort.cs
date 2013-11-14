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
    using System.Collections.ObjectModel;
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
        /// The edges from each node.
        /// </summary>
        private List<SortedSet<int>> edgesFrom;

        /// <summary>
        /// The edges into each node.
        /// </summary>
        private List<SortedSet<int>> edgesInto;

        /// <summary>
        /// The nodes dictionary. Maps nodes to IDs.
        /// </summary>
        private Dictionary<T, int> nodesDict;

        /// <summary>
        /// The nodes list. Maps IDs to their nodes.
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
            SubGroups = new List<IEnumerable<T>>();
        }

        public PriorityTopologicalSort(IEnumerable<IEnumerable<T>> subGroups) : this()
        {
            AddRange(subGroups);
        }

        #endregion

        #region Properties

        /// <summary>
        ///     Gets or sets the sub groups.
        ///     The order of the items in the sub group is important and will preserved in the final merged list.
        /// </summary>
        public IEnumerable<IEnumerable<T>> SubGroups { get; private set; }

        #endregion

        #region Methods

        /// <summary>
        /// The add.
        /// </summary>
        /// <param name="newGroup">
        /// The new group.
        /// </param>
        public void Add(IEnumerable<T> newGroup)
        {
            (SubGroups as List<IEnumerable<T>>).Add(newGroup);
        }

        /// <summary>
        /// The add range.
        /// </summary>
        /// <param name="newGroups">
        /// The new groups.
        /// </param>
        public void AddRange(IEnumerable<IEnumerable<T>> newGroups)
        {
            (SubGroups as List<IEnumerable<T>>).AddRange(newGroups);
        }

        /// <summary>
        /// The can sort.
        /// </summary>
        /// <param name="subGroup">
        /// The sub list.
        /// </param>
        /// <returns>
        /// The <see cref="bool"/>.
        /// </returns>
        public bool CanSort(IList<T> subGroup)
        {
            if (subGroup == null)
            {
                return false;
            }

            var topSort = new PriorityTopologicalSort<T>();
            topSort.SubGroups = SubGroups.ConcatWith(subGroup);

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

            for (var i = 1; i < SubGroups.Count(); i++)
            {
                var topSort = new PriorityTopologicalSort<T>();
                topSort.SubGroups = SubGroups.Take(i).ToList();

                var sortedList = topSort.Sort();

                if (sortedList != null)
                {
                    continue;
                }

                var subList = SubGroups.Take(i).Reverse().ToList();

                for (var j = 1; j < subList.Count; j++)
                {
                    topSort = new PriorityTopologicalSort<T>();
                    topSort.SubGroups = SubGroups.Take(j).ToList();

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

            foreach (var yarn in SubGroups)
            {
                int next = -1;
                int prev = -1;
                
                foreach (var node in yarn)
                {
                    if (!nodesDict.ContainsKey(node))
                    {
                        nodesDict.Add(node, nodesDict.Count);
                        nodesList.Add(node);
                        edgesFrom.Add(new SortedSet<int>());
                        edgesInto.Add(new SortedSet<int>());
                    }

                    next = nodesDict[node];

                    if (prev >= 0)
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

    public static class Extensions
    {
        public static ReadOnlyCollection<T> AsReadOnly<T>(this IList<T> list)
        {
            return new ReadOnlyCollection<T>(list);
        }

        public static IEnumerable<T> ConcatWith<T>(this IEnumerable<T> coll, T elem)
        {
            return coll.Concat(new[] { elem });
        }
    }
}

