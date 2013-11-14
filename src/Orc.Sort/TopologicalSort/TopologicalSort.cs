// --------------------------------------------------------------------------------------------------------------------
// <copyright file="TopologicalSort.cs" company="orc">
//   
// </copyright>
// <summary>
//   Uses a standard topological sort to order sequences.
// </summary>
// --------------------------------------------------------------------------------------------------------------------

namespace Orc.Sort.TopologicalSort
{
    using System;
    using System.Collections.Generic;
    using System.Linq;

    /// <summary>
    /// Standard topological sort.
    /// Should be faster than the PriorityTopologicalSort
    /// </summary>
    /// <typeparam name="T">
    /// </typeparam>
    public class TopologicalSort<T>
        where T : IEquatable<T>
    {
        #region Fields

        /// <summary>
        /// The edges from each node.
        /// </summary>
        private List<HashSet<int>> edgesFrom;

        /// <summary>
        /// The edges into each node.
        /// </summary>
        private List<HashSet<int>> edgesInto;

        /// <summary>
        /// The nodes dictionary. Maps nodes to IDs.
        /// </summary>
        private Dictionary<T, int> nodesDict;

        /// <summary>
        /// The nodes list. Maps IDs to their nodes.
        /// </summary>
        private List<T> nodesList;

        /// <summary>
        /// The nodes path.
        /// </summary>
        private List<T> nodesPath;

        #endregion

        #region Constructors

        /// <summary>
        /// Initializes a new instance of the <see cref="PriorityTopologicalSort{T}"/> class.
        /// </summary>
        public TopologicalSort()
        {
            Sequences = new List<IEnumerable<T>>();
            UsesPriority = false;
            UsesTracking = false;
        }

        public TopologicalSort(IEnumerable<IEnumerable<T>> sequences)
            : this()
        {
            AddRange(sequences);
        }

        #endregion

        #region Properties

        /// <summary>
        ///     Gets or sets the sub groups.
        ///     The order of the items in the sub group is important and will preserved in the final merged list.
        /// </summary>
        public IEnumerable<IEnumerable<T>> Sequences { get; private set; }

        /// <summary>
        /// Does the sorter use group add order priority?
        /// </summary>
        public Boolean UsesPriority { get; protected set; }

        /// <summary>
        /// Does the sorter use graph component tracking?
        /// </summary>
        public Boolean UsesTracking { get; protected set; }

        #endregion

        #region Methods

        /// <summary>
        /// The add.
        /// </summary>
        /// <param name="newSequence">
        /// The new group.
        /// </param>
        public void Add(IEnumerable<T> newSequence)
        {
            (Sequences as List<IEnumerable<T>>).Add(newSequence);
        }

        /// <summary>
        /// The add range.
        /// </summary>
        /// <param name="newSequences">
        /// The new groups.
        /// </param>
        public void AddRange(IEnumerable<IEnumerable<T>> newSequences)
        {
            (Sequences as List<IEnumerable<T>>).AddRange(newSequences);
        }

        /// <summary>
        /// The can sort.
        /// </summary>
        /// <param name="sequence">
        /// The sub list.
        /// </param>
        /// <returns>
        /// The <see cref="bool"/>.
        /// </returns>
        public bool CanSort(IEnumerable<T> sequence)
        {
            if (sequence == null)
            {
                return false;
            }

            var topSort = new TopologicalSort<T>();
            topSort.Sequences = Sequences.ConcatWith(sequence);

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

            for (var i = 1; i < Sequences.Count(); i++)
            {
                var topSort = new TopologicalSort<T>();
                topSort.Sequences = Sequences.Take(i).ToList();

                var sortedList = topSort.Sort();

                if (sortedList != null)
                {
                    continue;
                }

                var subList = Sequences.Take(i).Reverse().ToList();

                for (var j = 1; j < subList.Count; j++)
                {
                    topSort = new TopologicalSort<T>();
                    topSort.Sequences = Sequences.Take(j).ToList();

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

            edgesFrom = new List<HashSet<int>>();
            edgesInto = new List<HashSet<int>>();

            foreach (var sequence in Sequences)
            {
                int next = -1;
                int prev = -1;

                foreach (var node in sequence)
                {
                    if (!nodesDict.ContainsKey(node))
                    {
                        nodesDict.Add(node, nodesDict.Count);
                        nodesList.Add(node);
                        edgesFrom.Add(new HashSet<int>());
                        edgesInto.Add(new HashSet<int>());
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
            ISet<int> nodesNext;

            if (UsesPriority)
            {
                nodesNext = new SortedSet<int>();
            }
            else
            {
                nodesNext = new HashSet<int>();
            }

            nodesNext.UnionWith(Enumerable.Range(0, nodesDict.Count).Where(node => edgesInto[node].Count == 0));

            while (nodesNext.Count > 0)
            {
                int next = -1;

                if (UsesPriority)
                {
                    next = nodesNext.Max();
                }
                else
                {
                    next = nodesNext.First();
                }

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

