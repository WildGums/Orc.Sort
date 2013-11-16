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
        public TopologicalSort(bool usesPriority = false, bool usesTracking = false, IEnumerable<IEnumerable<T>> sequences = null)
        {
            Sequences = new List<IEnumerable<T>>();

            if (sequences != null)
            {
                AddRange(sequences);
            }
            
            UsesPriority = usesPriority;
            UsesTracking = usesTracking;
        }
        #endregion

        #region Properties

        /// <summary>
        ///     Gets the sub groups.
        ///     The order of the items in the sub group is important and will preserved in the final merged list.
        /// </summary>
        public IList<IEnumerable<T>> Sequences { get; private set; }

        /// <summary>
        /// Gets or sets a value indicating whether priority will be used or not.
        /// </summary>
        public bool UsesPriority { get; protected set; }

        /// <summary>
        /// Gets or sets a value indicating whether tracking will be used or not.
        /// </summary>
        public bool UsesTracking { get; protected set; }

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
            Sequences.Add(newSequence);
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
            topSort.Sequences = Sequences.ConcatWith(sequence).ToList();

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
        /// Returns the first pair of sequences that are in conflict with each other.
        /// </summary>
        /// <returns>
        /// The <see cref="List"/>.
        /// </returns>
        public IList<IEnumerable<T>> GetConflicts()
        {
            var conflictLists = new List<IEnumerable<T>>();

            var topSort = new TopologicalSort<T>();

            for (var i = 0; i < Sequences.Count; i++)
            {
                topSort.Add(Sequences[i]);

                var sortedList = topSort.Sort();

                if (sortedList != null)
                {
                    continue;
                }

                topSort = new TopologicalSort<T>();
                topSort.Add(Sequences[i]);

                for (var j = 0; j < i; j++)
                {
                    
                    topSort.Add(Sequences[j]);

                    sortedList = topSort.Sort();

                    if (sortedList != null)
                    {
                        continue;
                    }

                    conflictLists.Add(Sequences[j].ToList());
                    conflictLists.Add(Sequences[i].ToList());
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
                if (!sequence.Any())
                {
                    continue;
                }

                int next = 0;
                int prev = NodeKey(sequence.First());

                foreach (var node in sequence.Skip(1))
                {
                    next = NodeKey(node);

                    edgesFrom[next].Add(prev);
                    edgesInto[prev].Add(next);

                    prev = next;
                }
            }
        }

        /// <summary>
        /// The node key.
        /// </summary>
        private int NodeKey(T node)
        {
            int key;

            if (!nodesDict.TryGetValue(node, out key))
            {
                key = nodesDict.Count;

                nodesDict.Add(node, key);
                nodesList.Add(node);
                edgesFrom.Add(new HashSet<int>());
                edgesInto.Add(new HashSet<int>());
            }

            return key;
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

