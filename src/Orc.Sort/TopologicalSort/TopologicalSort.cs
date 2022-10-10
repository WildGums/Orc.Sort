namespace Orc.Sort.TopologicalSort
{
    using System;
    using System.Collections.Generic;
    using System.Linq;
    using Catel;

    /// <summary>
    /// Standard topological sort.
    /// Should be faster than the PriorityTopologicalSort
    /// </summary>
    /// <typeparam name="T">
    /// </typeparam>
    public class TopologicalSort<T>
        where T : notnull
    {
        #region Fields
        /// <summary>
        /// The edges from each node.
        /// </summary>
        protected List<HashSet<int>> edgesFrom;

        /// <summary>
        /// The edges into each node.
        /// </summary>
        protected List<HashSet<int>> edgesInto;

        /// <summary>
        /// First conflicting sequence.
        /// </summary>
        protected IEnumerable<T>? firstConflict;

        /// <summary>
        /// The nodes dictionary. Maps nodes to IDs.
        /// </summary>
        protected Dictionary<T, int> nodesDict;

        /// <summary>
        /// The nodes list. Maps IDs to their nodes.
        /// </summary>
        protected List<T> nodesList;

        /// <summary>
        /// List of nodes in the sort order.
        /// </summary>
        protected List<T>? nodesSort;

        /// <summary>
        /// The edges from each node - transitive closure.
        /// </summary>
        protected List<HashSet<int>>? transFrom;

        /// <summary>
        /// The edges into each node - transitive closure.
        /// </summary>
        protected List<HashSet<int>>? transInto;
        #endregion

        #region Constructors
        /// <summary>
        /// Initializes a new instance of the <see cref="PriorityTopologicalSort{T}"/> class.
        /// </summary>
        public TopologicalSort(bool usesPriority = false, bool usesTracking = false, IEnumerable<IEnumerable<T>>? sequences = null)
        {
            nodesDict = new Dictionary<T, int>();
            nodesList = new List<T>();

            edgesInto = new List<HashSet<int>>();
            edgesFrom = new List<HashSet<int>>();

            if (usesTracking)
            {
                transInto = new List<HashSet<int>>();
                transFrom = new List<HashSet<int>>();
            }

            UsesPriority = usesPriority;
            UsesTracking = usesTracking;

            Sequences = new List<IEnumerable<T>>();

            if (sequences is not null)
            {
                AddRange(sequences);
            }
        }

        public TopologicalSort(TopologicalSort<T> baseSort, bool? usesPriority = null, bool? usesTracking = null)
        {
            nodesDict = new Dictionary<T, int>(baseSort.nodesDict);
            nodesList = new List<T>(baseSort.nodesList);

            edgesInto = new List<HashSet<int>>(baseSort.edgesInto.Select(hash => new HashSet<int>(hash)));
            edgesFrom = new List<HashSet<int>>(baseSort.edgesFrom.Select(hash => new HashSet<int>(hash)));

            UsesPriority = usesPriority ?? baseSort.UsesPriority;
            UsesTracking = usesTracking ?? baseSort.UsesTracking;

            if (UsesTracking)
            {
                if (baseSort.transInto is not null)
                {
                    transInto = new List<HashSet<int>>(baseSort.transInto.Select(hash => new HashSet<int>(hash)));
                }

                if (baseSort.transFrom is not null)
                {
                    transFrom = new List<HashSet<int>>(baseSort.transFrom.Select(hash => new HashSet<int>(hash)));
                }
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
        public IEnumerable<IEnumerable<T>> Sequences { get; protected set; }

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
        public virtual void Add(IEnumerable<T> sequence)
        {
            ArgumentNullException.ThrowIfNull(sequence);

            ((IList<IEnumerable<T>>)Sequences).Add(sequence);

            if (!sequence.Any())
            {
                return;
            }

            int next = 0;
            int prev = NodeKey(sequence.First());

            foreach (var node in sequence.Skip(1))
            {
                next = NodeKey(node);

                edgesInto[next].Add(prev);
                edgesFrom[prev].Add(next);

                if (UsesTracking && 
                    transInto is not null &&
                    transFrom is not null)
                {
                    foreach (int temp in transInto[prev])
                    {
                        transFrom[temp].UnionWith(transFrom[next]);
                    }
                    foreach (int temp in transFrom[next])
                    {
                        transInto[temp].UnionWith(transInto[prev]);
                    }
                    if (transInto[prev].Contains(next))
                    {
                        firstConflict = firstConflict ?? sequence;
                    }
                }

                prev = next;
            }

            nodesSort = null;
        }

        /// <summary>
        /// The add range.
        /// </summary>
        /// <param name="newSequences">
        /// The new groups.
        /// </param>
        public virtual void AddRange(IEnumerable<IEnumerable<T>> newSequences)
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
            if (sequence is null)
            {
                return false;
            }

            if (UsesTracking)
            {
                var tempInto = new HashSet<int>();
                var tempSeen = new HashSet<int>();
                var tempDict = new Dictionary<T, int>(nodesDict);

                if (transInto is not null)
                {
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

                        if (key < transInto.Count)
                        {
                            tempInto.UnionWith(transInto[key]);
                            tempInto.Remove(key);
                        }
                    }
                }

                tempInto.IntersectWith(tempSeen);

                return !tempInto.Any();
            }
            else
            {
                var newSort = new TopologicalSort<T>(this, false, false);
                newSort.Add(sequence);

                return newSort.Sort() is not null;
            }
        }

        /// <summary>
        /// The sort.
        /// </summary>
        /// <returns>
        /// The <see cref="List{T}"/>.
        /// </returns>
        public IList<T>? Sort()
        {
            if (nodesSort is not null)
            {
                return nodesSort.AsReadOnly();
            }

            var nodesPath = new List<T>();
            var countFrom = edgesFrom.Select(e => e.Count).ToList();

            ISet<int> nodesNext;

            if (UsesPriority)
            {
                nodesNext = new SortedSet<int>();
            }
            else
            {
                nodesNext = new HashSet<int>();
            }

            nodesNext.UnionWith(Enumerable.Range(0, nodesDict.Count).Where(node => countFrom[node] == 0));

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

                foreach (int node in edgesInto[next])
                {
                    countFrom[node] -= 1;

                    if (countFrom[node] == 0)
                    {
                        nodesNext.Add(node);
                    }
                }
            }

            nodesPath.Reverse();

            if (nodesPath.Count == nodesList.Count)
            {
                nodesSort = nodesPath;
                return nodesSort.AsReadOnly();
            }
            else
            {
                return null;
            }
        }

        /// <summary>
        /// Returns the first pair of sequences that are in conflict with each other.
        /// </summary>
        /// <returns>The <see cref="List{T}" />.</returns>
        public IList<IEnumerable<T>> GetConflicts()
        {
            var conflictLists = new List<IEnumerable<T>>();

            if (UsesTracking)
            {
                if (firstConflict is not null)
                {
                    conflictLists.Add(Sequences.First(s => !CanSort(s)));
                    conflictLists.Add(firstConflict);
                }
                return conflictLists;
            }
            else
            {
                var newSort = new TopologicalSort<T>();

                var sequences = (IList<IEnumerable<T>>)Sequences;

                for (var i = 0; i < sequences.Count; i++)
                {
                    newSort.Add(sequences[i]);

                    if (newSort.Sort() is not null)
                    {
                        continue;
                    }

                    newSort = new TopologicalSort<T>();
                    newSort.Add(sequences[i]);

                    for (var j = 0; j < i; j++)
                    {
                        newSort.Add(sequences[j]);

                        if (newSort.Sort() is not null)
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
        protected virtual int NodeKey(T node)
        {
            int key;

            if (!nodesDict.TryGetValue(node, out key))
            {
                key = nodesDict.Count;

                nodesDict.Add(node, key);
                nodesList.Add(node);

                edgesInto.Add(new HashSet<int>());
                edgesFrom.Add(new HashSet<int>());

                if (UsesTracking &&
                    transInto is not null &&
                    transFrom is not null)
                {
                    transInto.Add(new HashSet<int>(new int[] { key }));
                    transFrom.Add(new HashSet<int>(new int[] { key }));
                }
            }

            return key;
        }

        /// <summary>
        /// The node key - safe.
        /// </summary>
        protected int NodeKeySafe(T node, Dictionary<T, int> tempDict)
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
        /// Returns the list of all nodes that the given node depends on. (Nodes that must come before the given node.)
        /// </summary>
        public IEnumerable<T> GetPrecedents(T node, bool immediate = false, bool terminating = false)
        {
            return GetPrecedents(nodesDict[node], immediate, terminating).Select(next => nodesList[next]);
        }

        protected IEnumerable<int> GetPrecedents(int node, bool immediate, bool terminating)
        {
            return GetRelated(node, immediate, terminating, edgesInto, transInto);
        }

        /// <summary>
        /// Returns the list of all nodes that depend on the given node. (Nodes that must come after the given node.)
        /// </summary>
        public IEnumerable<T> GetDependents(T node, bool immediate = false, bool terminating = false)
        {
            return GetDependents(nodesDict[node], immediate, terminating).Select(next => nodesList[next]);
        }

        protected IEnumerable<int> GetDependents(int node, bool immediate, bool terminating)
        {
            return GetRelated(node, immediate, terminating, edgesFrom, transFrom);
        }

        /// <summary>
        /// Returns the list of all nodes that are related to the given node.
        /// </summary>
        protected IEnumerable<int> GetRelated(int node, bool immediate, bool terminating, List<HashSet<int>> edgesData, List<HashSet<int>>? transData = null, bool lazy = false)
        {
            IEnumerable<int> nodesData;

            if (immediate)
            {
                nodesData = edgesData[node];
            }
            else if (UsesTracking && transData is not null)
            {
                nodesData = transData[node];
            }
            else if (lazy)
            {
                nodesData = EdgesWalkLazy(node, edgesData);
            }
            else
            {
                nodesData = EdgesWalk(node, edgesData);
            }

            if (terminating)
            {
                nodesData = nodesData.Where(next => ((next != node) && (edgesData[next].Count == 0)));
            }
            else
            {
                nodesData = nodesData.ExceptItem(node);
            }

            return nodesData;
        }

        /// <summary>
        /// Walks the edges of the given node recursively, and returns a set of visited nodes.
        /// </summary>
        protected HashSet<int> EdgesWalk(int node, List<HashSet<int>> edgesWalk)
        {
            var nodesWalk = new HashSet<int>();
            var nodesSeen = new HashSet<int>();

            nodesWalk.Add(node);

            while (nodesWalk.Any())
            {
                node = nodesWalk.First();

                if (nodesSeen.Add(node))
                {
                    nodesWalk.UnionWith(edgesWalk[node]);
                }

                nodesWalk.Remove(node);
            }

            return nodesSeen;
        }

        /// <summary>
        /// Walks the edges of the given node recursively, and yields each visited node.
        /// </summary>
        protected IEnumerable<int> EdgesWalkLazy(int node, List<HashSet<int>> edgesWalk)
        {
            var nodesWalk = new HashSet<int>();
            var nodesSeen = new HashSet<int>();

            nodesWalk.Add(node);

            while (nodesWalk.Any())
            {
                node = nodesWalk.First();

                if (nodesSeen.Add(node))
                {
                    nodesWalk.UnionWith(edgesWalk[node]);
                    yield return node;
                }

                nodesWalk.Remove(node);
            }
        }
        #endregion
    }
}
