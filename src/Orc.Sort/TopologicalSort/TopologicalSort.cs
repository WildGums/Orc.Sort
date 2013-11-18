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
    {
        #region Fields

        /// <summary>
        /// The nodes dictionary. Maps nodes to IDs.
        /// </summary>
        private Dictionary<T, int> nodesDict;

        /// <summary>
        /// The nodes list. Maps IDs to their nodes.
        /// </summary>
        private List<T> nodesList;

        /// <summary>
        /// The edges from each node.
        /// </summary>
        private List<HashSet<int>> edgesFrom;

        /// <summary>
        /// The edges into each node.
        /// </summary>
        private List<HashSet<int>> edgesInto;

        /// <summary>
        /// The edges from each node - transitive closure.
        /// </summary>
        private List<HashSet<int>> transFrom;

        /// <summary>
        /// The edges into each node - transitive closure.
        /// </summary>
        private List<HashSet<int>> transInto;

        private IEnumerable<T> firstConflict;

        #endregion

        #region Constructors

        /// <summary>
        /// Initializes a new instance of the <see cref="PriorityTopologicalSort{T}"/> class.
        /// </summary>
        public TopologicalSort(bool usesPriority = false, bool usesTracking = false, IEnumerable<IEnumerable<T>> sequences = null)
        {
            nodesDict = new Dictionary<T, int>();
            nodesList = new List<T>();

            edgesFrom = new List<HashSet<int>>();
            edgesInto = new List<HashSet<int>>();

            if (usesTracking)
            {
                transFrom = new List<HashSet<int>>();
                transInto = new List<HashSet<int>>();
            }

            UsesPriority = usesPriority;
            UsesTracking = usesTracking;

            Sequences = new List<IEnumerable<T>>();

            if (sequences != null)
            {
                AddRange(sequences);
            }
        }

        public TopologicalSort(TopologicalSort<T> baseSort, bool? usesPriority = null, bool? usesTracking = null)
        {
            nodesDict = new Dictionary<T, int>(baseSort.nodesDict);
            nodesList = new List<T>(baseSort.nodesList);

            edgesFrom = new List<HashSet<int>>(baseSort.edgesFrom.Select(hash => new HashSet<int>(hash)));
            edgesInto = new List<HashSet<int>>(baseSort.edgesInto.Select(hash => new HashSet<int>(hash)));

            UsesPriority = usesPriority ?? baseSort.UsesPriority;
            UsesTracking = usesTracking ?? baseSort.UsesTracking;

            if (UsesTracking)
            {
                transFrom = new List<HashSet<int>>(baseSort.transFrom.Select(hash => new HashSet<int>(hash)));
                transInto = new List<HashSet<int>>(baseSort.transInto.Select(hash => new HashSet<int>(hash)));
            }

            Sequences = baseSort.Sequences.ToList();
        }

        #endregion

        #region Properties

        /// <summary>
        /// Gets the sequences.
        /// The order of items within each sequence is important and will preserved in the final merged sequence.
        /// If the sorter uses priority, additionally the appearance order of each item will be used if possible.
        /// </summary>
        public IEnumerable<IEnumerable<T>> Sequences { get; private set; }

        /// <summary>
        /// Gets the value indicating whether priority will be used or not.
        /// </summary>
        public bool UsesPriority { get; protected set; }

        /// <summary>
        /// Gets the value indicating whether tracking will be used or not.
        /// </summary>
        public bool UsesTracking { get; protected set; }

        #endregion

        #region Methods

        /// <summary>
        /// The add.
        /// </summary>
        /// <param name="sequence">
        /// The new group.
        /// </param>
        public void Add(IEnumerable<T> sequence)
        {
            (Sequences as IList<IEnumerable<T>>).Add(sequence);

            if (!sequence.Any())
            {
                return;
            }

            int next = 0;
            int prev = NodeKey(sequence.First());

            foreach (var node in sequence.Skip(1))
            {
                next = NodeKey(node);

                edgesFrom[next].Add(prev);
                edgesInto[prev].Add(next);

                if (UsesTracking)
                {
                    foreach (int temp in transFrom[prev])
                    {
                        transInto[temp].UnionWith(transInto[next]);
                    }
                    foreach (int temp in transInto[next])
                    {
                        transFrom[temp].UnionWith(transFrom[prev]);
                    }
                    if (transFrom[prev].Contains(next))
                    {
                        firstConflict = firstConflict ?? sequence;
                    }
                }

                prev = next;
            }
        }

        /// <summary>
        /// The add range.
        /// </summary>
        /// <param name="newSequences">
        /// The new groups.
        /// </param>
        public void AddRange(IEnumerable<IEnumerable<T>> newSequences)
        {
            foreach (var sequence in newSequences)
            {
                Add(sequence);
            }
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

            if (UsesTracking)
            {
                var tempFrom = new HashSet<int>();
                var tempSeen = new HashSet<int>();
                var tempDict = new Dictionary<T, int>(nodesDict);

                foreach (var node in sequence.Reverse())
                {
                    var key = NodeKeySafe(node, tempDict);

                    if (tempSeen.Contains(key))
                    {
                        return false;
                    }
                    else
                    {
                        tempSeen.Add(key);
                    }

                    if (key < transFrom.Count)
                    {
                        tempFrom.UnionWith(transFrom[key]);
                        tempFrom.Remove(key);
                    }
                }

                tempFrom.IntersectWith(tempSeen);

                return !tempFrom.Any();
            }
            else
            {
                var newSort = new TopologicalSort<T>(this, false, false);
                newSort.Add(sequence);

                return newSort.Sort() != null;
            }
        }

        /// <summary>
        /// The sort.
        /// </summary>
        /// <returns>
        /// The <see cref="List"/>.
        /// </returns>
        public IList<T> Sort()
        {
            var nodesPath = new List<T>();
            var countInto = edgesInto.Select(e => e.Count).ToList();

            ISet<int> nodesNext;

            if (UsesPriority)
            {
                nodesNext = new SortedSet<int>();
            }
            else
            {
                nodesNext = new HashSet<int>();
            }

            nodesNext.UnionWith(Enumerable.Range(0, nodesDict.Count).Where(node => countInto[node] == 0));

            while (nodesNext.Count > 0)
            {
                int next;

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
                    countInto[node] -= 1;

                    if (countInto[node] == 0)
                    {
                        nodesNext.Add(node);
                    }
                }
            }

            nodesPath.Reverse();

            if (nodesPath.Count == nodesList.Count)
            {
                return nodesPath;
            }
            else
            {
                return null;
            }
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

            if (UsesTracking)
            {
                if (firstConflict != null)
                {
                    conflictLists.Add(Sequences.First(s => !CanSort(s)));
                    conflictLists.Add(firstConflict);
                }
                return conflictLists;
            }
            else
            {
                var newSort = new TopologicalSort<T>();

                var sequences = (Sequences as IList<IEnumerable<T>>);

                for (var i = 0; i < sequences.Count; i++)
                {
                    newSort.Add(sequences[i]);

                    if (newSort.Sort() != null)
                    {
                        continue;
                    }

                    newSort = new TopologicalSort<T>();
                    newSort.Add(sequences[i]);

                    for (var j = 0; j < i; j++)
                    {
                        newSort.Add(sequences[j]);

                        if (newSort.Sort() != null)
                        {
                            continue;
                        }

                        conflictLists.Add(sequences[j].ToList());
                        conflictLists.Add(sequences[i].ToList());

                        break;
                    }

                    break;
                }

                return conflictLists;
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

                if (UsesTracking)
                {
                    transFrom.Add(new HashSet<int>(new int[] { key }));
                    transInto.Add(new HashSet<int>(new int[] { key }));
                }
            }

            return key;
        }

        /// <summary>
        /// The node key - safe.
        /// </summary>
        private int NodeKeySafe(T node, Dictionary<T, int> tempDict)
        {
            int key;

            if (!tempDict.TryGetValue(node, out key))
            {
                key = tempDict.Count;
                tempDict[node] = key;
            }

            return key;
        }

        
        /// <summary>
        /// Returns the list nodes that the given node depends on. (Nodes that must come after the given node.)
        /// </summary>
        public IEnumerable<T> GetDependencies(T node)
        {
            if (!UsesTracking)
            {
                throw new InvalidOperationException("tracking is not enabled");
            }

            int next = nodesDict[node];

            return transFrom[next].Select(prev => nodesList[prev]);
        }

        /// <summary>
        /// Returns the list of all nodes that depend on the given node. (Nodes that the given node must come before).
        /// </summary>
        public IEnumerable<T> GetDependenciesReverse(T node)
        {
            if (!UsesTracking)
            {
                throw new InvalidOperationException("tracking is not enabled");
            }

            int prev = nodesDict[node];

            return transInto[prev].Select(next => nodesList[next]);
        }

        #endregion
    }
}

