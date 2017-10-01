[assembly: System.Resources.NeutralResourcesLanguageAttribute("en-US")]
[assembly: System.Runtime.InteropServices.ComVisibleAttribute(false)]
[assembly: System.Runtime.Versioning.TargetFrameworkAttribute(".NETFramework,Version=v4.6", FrameworkDisplayName=".NET Framework 4.6")]


public class static LoadAssembliesOnStartup { }
public class static ModuleInitializer
{
    public static void Initialize() { }
}
namespace Orc.Sort
{
    
    public class static IEnumerableExtensions
    {
        [System.Runtime.CompilerServices.IteratorStateMachineAttribute(typeof(Orc.Sort.IEnumerableExtensions.<EnumerateFrom>d__0<>))]
        public static System.Collections.Generic.IEnumerable<T> EnumerateFrom<T>(this System.Collections.Generic.IEnumerable<T> items, T startValue, bool isCyclic = True)
            where T : System.IEquatable<> { }
        public static System.Collections.Generic.IEnumerable<T> MergeSorted<T>(this System.Collections.Generic.IEnumerable<System.Collections.Generic.IEnumerable<T>> sortedEnumerables, bool distinct = False)
            where T : System.IComparable<> { }
        [System.Runtime.CompilerServices.IteratorStateMachineAttribute(typeof(Orc.Sort.IEnumerableExtensions.<MergeSorted>d__2<>))]
        public static System.Collections.Generic.IEnumerable<T> MergeSorted<T>(this System.Collections.Generic.IEnumerable<System.Collections.Generic.IEnumerable<T>> sortedEnumerables, System.Collections.Generic.IComparer<T> itemComparer, bool distinct = False) { }
        [System.Runtime.CompilerServices.IteratorStateMachineAttribute(typeof(Orc.Sort.IEnumerableExtensions.<MergeSortedMany>d__3<>))]
        public static System.Collections.Generic.IEnumerable<T> MergeSortedMany<T>(this System.Collections.Generic.IEnumerable<System.Collections.Generic.IEnumerable<T>> sortedEnumerables, System.Collections.Generic.IComparer<T> itemComparer) { }
    }
    public interface ISorter
    {
        void Sort(System.Collections.IList list);
    }
    public interface ISorter<T>
    
    {
        void Sort(System.Collections.Generic.IList<T> list);
    }
    public interface ISwap
    {
        void Set(System.Collections.IList array, int left, int right);
        void Set(System.Collections.IList array, int left, object obj);
        void Swap(System.Collections.IList array, int left, int right);
    }
    public interface ISwap<T>
    
    {
        void Set(System.Collections.Generic.IList<T> array, int left, int right);
        void Set(System.Collections.Generic.IList<T> array, int left, T obj);
        void Swap(System.Collections.Generic.IList<T> array, int left, int right);
    }
}
namespace Orc.Sort.NSort
{
    
    public class BiDirectionalBubbleSort : Orc.Sort.NSort.SwapSorter
    {
        public BiDirectionalBubbleSort() { }
        public BiDirectionalBubbleSort(System.Collections.IComparer comparer, Orc.Sort.ISwap swapper) { }
        public override void Sort(System.Collections.IList list) { }
    }
    public class BubbleSorter : Orc.Sort.NSort.SwapSorter
    {
        public BubbleSorter() { }
        public BubbleSorter(System.Collections.IComparer comparer, Orc.Sort.ISwap swapper) { }
        public override void Sort(System.Collections.IList list) { }
    }
    public class ComboSort11 : Orc.Sort.NSort.SwapSorter
    {
        public ComboSort11() { }
        public ComboSort11(System.Collections.IComparer comparer, Orc.Sort.ISwap swapper) { }
        public override void Sort(System.Collections.IList list) { }
    }
    public class ComparableComparer : System.Collections.IComparer
    {
        public ComparableComparer() { }
        public int Compare(System.IComparable x, object y) { }
    }
    public class DefaultSwap : Orc.Sort.ISwap
    {
        public DefaultSwap() { }
        public void Set(System.Collections.IList array, int left, int right) { }
        public void Set(System.Collections.IList array, int left, object obj) { }
        public void Swap(System.Collections.IList array, int left, int right) { }
    }
    public class DoubleStorageMergeSort : Orc.Sort.NSort.SwapSorter
    {
        public DoubleStorageMergeSort() { }
        public DoubleStorageMergeSort(System.Collections.IComparer comparer, Orc.Sort.ISwap swapper) { }
        public override void Sort(System.Collections.IList list) { }
    }
    public class FastQuickSorter : Orc.Sort.NSort.SwapSorter
    {
        public FastQuickSorter() { }
        public FastQuickSorter(System.Collections.IComparer comparer, Orc.Sort.ISwap swapper) { }
        public override void Sort(System.Collections.IList list) { }
    }
    public class HeapSort : Orc.Sort.NSort.SwapSorter
    {
        public HeapSort() { }
        public HeapSort(System.Collections.IComparer comparer, Orc.Sort.ISwap swapper) { }
        public override void Sort(System.Collections.IList list) { }
    }
    public class InPlaceMergeSort : Orc.Sort.NSort.SwapSorter
    {
        public InPlaceMergeSort() { }
        public InPlaceMergeSort(System.Collections.IComparer comparer, Orc.Sort.ISwap swapper) { }
        public override void Sort(System.Collections.IList list) { }
    }
    public class InsertionSort : Orc.Sort.NSort.SwapSorter
    {
        public InsertionSort() { }
        public InsertionSort(System.Collections.IComparer comparer, Orc.Sort.ISwap swapper) { }
        public override void Sort(System.Collections.IList list) { }
    }
    public class OddEvenTransportSorter : Orc.Sort.NSort.SwapSorter
    {
        public OddEvenTransportSorter() { }
        public OddEvenTransportSorter(System.Collections.IComparer comparer, Orc.Sort.ISwap swapper) { }
        public override void Sort(System.Collections.IList list) { }
    }
    public class QuickSorter : Orc.Sort.NSort.SwapSorter
    {
        public QuickSorter() { }
        public QuickSorter(System.Collections.IComparer comparer, Orc.Sort.ISwap swapper) { }
        public override void Sort(System.Collections.IList array) { }
        public void Sort(System.Collections.IList array, int lower, int upper) { }
    }
    public class QuickSortWithBubbleSort : Orc.Sort.NSort.SwapSorter
    {
        public QuickSortWithBubbleSort() { }
        public QuickSortWithBubbleSort(System.Collections.IComparer comparer, Orc.Sort.ISwap swapper) { }
        public override void Sort(System.Collections.IList list) { }
    }
    public class SelectionSort : Orc.Sort.NSort.SwapSorter
    {
        public SelectionSort() { }
        public SelectionSort(System.Collections.IComparer comparer, Orc.Sort.ISwap swapper) { }
        public override void Sort(System.Collections.IList list) { }
    }
    public class ShakerSort : Orc.Sort.NSort.SwapSorter
    {
        public ShakerSort() { }
        public ShakerSort(System.Collections.IComparer comparer, Orc.Sort.ISwap swapper) { }
        public override void Sort(System.Collections.IList list) { }
    }
    public class ShearSorter
    {
        public ShearSorter() { }
    }
    public class ShellSort : Orc.Sort.NSort.SwapSorter
    {
        public ShellSort() { }
        public ShellSort(System.Collections.IComparer comparer, Orc.Sort.ISwap swapper) { }
        public override void Sort(System.Collections.IList list) { }
    }
    public abstract class SwapSorter : Orc.Sort.ISorter
    {
        public SwapSorter() { }
        public SwapSorter(System.Collections.IComparer comparer, Orc.Sort.ISwap swapper) { }
        public System.Collections.IComparer Comparer { get; set; }
        public Orc.Sort.ISwap Swapper { get; set; }
        public abstract void Sort(System.Collections.IList list);
    }
}
namespace Orc.Sort.NSort.Generic
{
    
    public class BiDirectionalBubbleSort<T> : Orc.Sort.NSort.Generic.SwapSorter<T>
    
    {
        public BiDirectionalBubbleSort() { }
        public BiDirectionalBubbleSort(System.Collections.Generic.IComparer<T> comparer, Orc.Sort.ISwap<T> swapper) { }
        public override void Sort(System.Collections.Generic.IList<T> list) { }
    }
    public class BubbleSorter<T> : Orc.Sort.NSort.Generic.SwapSorter<T>
    
    {
        public BubbleSorter() { }
        public BubbleSorter(System.Collections.Generic.IComparer<T> comparer, Orc.Sort.ISwap<T> swapper) { }
        public override void Sort(System.Collections.Generic.IList<T> list) { }
    }
    public class ComboSort11<T> : Orc.Sort.NSort.Generic.SwapSorter<T>
    
    {
        public ComboSort11() { }
        public ComboSort11(System.Collections.Generic.IComparer<T> comparer, Orc.Sort.ISwap<T> swapper) { }
        public override void Sort(System.Collections.Generic.IList<T> list) { }
    }
    public class ComparableComparer<T> : System.Collections.Generic.IComparer<T>
    
    {
        public ComparableComparer() { }
        public int Compare(System.IComparable<T> x, T y) { }
    }
    public class DefaultSwap<T> : Orc.Sort.ISwap<T>
    
    {
        public DefaultSwap() { }
        public void Set(System.Collections.Generic.IList<T> array, int left, int right) { }
        public void Set(System.Collections.Generic.IList<T> array, int left, T obj) { }
        public void Swap(System.Collections.Generic.IList<T> array, int left, int right) { }
    }
    public class DoubleStorageMergeSort<T> : Orc.Sort.NSort.Generic.SwapSorter<T>
    
    {
        public DoubleStorageMergeSort() { }
        public DoubleStorageMergeSort(System.Collections.Generic.IComparer<T> comparer, Orc.Sort.ISwap<T> swapper) { }
        public override void Sort(System.Collections.Generic.IList<T> list) { }
    }
    public class FastQuickSorter<T> : Orc.Sort.NSort.Generic.SwapSorter<T>
    
    {
        public FastQuickSorter() { }
        public FastQuickSorter(System.Collections.Generic.IComparer<T> comparer, Orc.Sort.ISwap<T> swapper) { }
        public override void Sort(System.Collections.Generic.IList<T> list) { }
    }
    public class HeapSort<T> : Orc.Sort.NSort.Generic.SwapSorter<T>
    
    {
        public HeapSort() { }
        public HeapSort(System.Collections.Generic.IComparer<T> comparer, Orc.Sort.ISwap<T> swapper) { }
        public override void Sort(System.Collections.Generic.IList<T> list) { }
    }
    public class InPlaceMergeSort<T> : Orc.Sort.NSort.Generic.SwapSorter<T>
    
    {
        public InPlaceMergeSort() { }
        public InPlaceMergeSort(System.Collections.Generic.IComparer<T> comparer, Orc.Sort.ISwap<T> swapper) { }
        public override void Sort(System.Collections.Generic.IList<T> list) { }
    }
    public class InsertionSort<T> : Orc.Sort.NSort.Generic.SwapSorter<T>
    
    {
        public InsertionSort() { }
        public InsertionSort(System.Collections.Generic.IComparer<T> comparer, Orc.Sort.ISwap<T> swapper) { }
        public override void Sort(System.Collections.Generic.IList<T> list) { }
    }
    public class OddEvenTransportSorter<T> : Orc.Sort.NSort.Generic.SwapSorter<T>
    
    {
        public OddEvenTransportSorter() { }
        public OddEvenTransportSorter(System.Collections.Generic.IComparer<T> comparer, Orc.Sort.ISwap<T> swapper) { }
        public override void Sort(System.Collections.Generic.IList<T> list) { }
    }
    public class QuickSorter<T> : Orc.Sort.NSort.Generic.SwapSorter<T>
    
    {
        public QuickSorter() { }
        public QuickSorter(System.Collections.Generic.IComparer<T> comparer, Orc.Sort.ISwap<T> swapper) { }
        public override void Sort(System.Collections.Generic.IList<T> array) { }
        public void Sort(System.Collections.Generic.IList<T> array, int lower, int upper) { }
    }
    public class QuickSortWithBubbleSort<T> : Orc.Sort.NSort.Generic.SwapSorter<T>
    
    {
        public QuickSortWithBubbleSort() { }
        public QuickSortWithBubbleSort(System.Collections.Generic.IComparer<T> comparer, Orc.Sort.ISwap<T> swapper) { }
        public override void Sort(System.Collections.Generic.IList<T> list) { }
    }
    public class SelectionSort<T> : Orc.Sort.NSort.Generic.SwapSorter<T>
    
    {
        public SelectionSort() { }
        public SelectionSort(System.Collections.Generic.IComparer<T> comparer, Orc.Sort.ISwap<T> swapper) { }
        public override void Sort(System.Collections.Generic.IList<T> list) { }
    }
    public class ShakerSort<T> : Orc.Sort.NSort.Generic.SwapSorter<T>
    
    {
        public ShakerSort() { }
        public ShakerSort(System.Collections.Generic.IComparer<T> comparer, Orc.Sort.ISwap<T> swapper) { }
        public override void Sort(System.Collections.Generic.IList<T> list) { }
    }
    public class ShearSorter<T>
    
    {
        public ShearSorter() { }
    }
    public class ShellSort<T> : Orc.Sort.NSort.Generic.SwapSorter<T>
    
    {
        public ShellSort() { }
        public ShellSort(System.Collections.Generic.IComparer<T> comparer, Orc.Sort.ISwap<T> swapper) { }
        public override void Sort(System.Collections.Generic.IList<T> list) { }
    }
    public abstract class SwapSorter<T> : Orc.Sort.ISorter<T>
    
    {
        public SwapSorter() { }
        public SwapSorter(System.Collections.Generic.IComparer<T> comparer, Orc.Sort.ISwap<T> swapper) { }
        public System.Collections.Generic.IComparer<T> Comparer { get; set; }
        public Orc.Sort.ISwap<T> Swapper { get; set; }
        public abstract void Sort(System.Collections.Generic.IList<T> list);
    }
}
namespace Orc.Sort.TemplateSort
{
    
    public class static TemplateSort
    {
        public static System.Collections.Generic.IEnumerable<T> SortAccordingTo<T>(this System.Collections.Generic.IEnumerable<T> sourceCollection, System.Collections.Generic.IEnumerable<T> templateCollection, bool includeNoMatches = True, System.Collections.Generic.IEqualityComparer<T> comparer = null) { }
        public static System.Collections.Generic.IEnumerable<TSource> SortAccordingTo<TSource, TKey>(this System.Collections.Generic.IEnumerable<TSource> sourceCollection, System.Collections.Generic.IEnumerable<TKey> templateCollection, System.Func<TSource, TKey> keySelector, bool includeNoMatches = True, System.Collections.Generic.IEqualityComparer<TKey> comparer = null) { }
    }
}
namespace Orc.Sort.TopologicalSort
{
    
    public class static CollectionExtensions
    {
        public static System.Collections.ObjectModel.ReadOnlyCollection<T> AsReadOnly<T>(this System.Collections.Generic.IList<T> list) { }
        public static System.Collections.Generic.IEnumerable<T> ConcatItem<T>(this System.Collections.Generic.IEnumerable<T> collection, T item) { }
        public static System.Collections.Generic.IEnumerable<T> ExceptItem<T>(this System.Collections.Generic.IEnumerable<T> collection, T item) { }
    }
    public class PriorityTopologicalSort<T> : Orc.Sort.TopologicalSort.TopologicalSort<T>
        where T : System.IEquatable<>
    {
        public PriorityTopologicalSort() { }
    }
    public class TopologicalSort<T>
    
    {
        protected System.Collections.Generic.List<System.Collections.Generic.HashSet<int>> edgesFrom;
        protected System.Collections.Generic.List<System.Collections.Generic.HashSet<int>> edgesInto;
        protected System.Collections.Generic.IEnumerable<T> firstConflict;
        protected System.Collections.Generic.Dictionary<T, int> nodesDict;
        protected System.Collections.Generic.List<T> nodesList;
        protected System.Collections.Generic.List<T> nodesSort;
        protected System.Collections.Generic.List<System.Collections.Generic.HashSet<int>> transFrom;
        protected System.Collections.Generic.List<System.Collections.Generic.HashSet<int>> transInto;
        public TopologicalSort(bool usesPriority = False, bool usesTracking = False, System.Collections.Generic.IEnumerable<System.Collections.Generic.IEnumerable<T>> sequences = null) { }
        public TopologicalSort(Orc.Sort.TopologicalSort.TopologicalSort<T> baseSort, System.Nullable<bool> usesPriority = null, System.Nullable<bool> usesTracking = null) { }
        public System.Collections.Generic.IEnumerable<System.Collections.Generic.IEnumerable<T>> Sequences { get; set; }
        public bool UsesPriority { get; set; }
        public bool UsesTracking { get; set; }
        public virtual void Add(System.Collections.Generic.IEnumerable<T> sequence) { }
        public virtual void AddRange(System.Collections.Generic.IEnumerable<System.Collections.Generic.IEnumerable<T>> newSequences) { }
        public bool CanSort(System.Collections.Generic.IEnumerable<T> sequence) { }
        protected System.Collections.Generic.HashSet<int> EdgesWalk(int node, System.Collections.Generic.List<System.Collections.Generic.HashSet<int>> edgesWalk) { }
        [System.Runtime.CompilerServices.IteratorStateMachineAttribute(typeof(Orc.Sort.TopologicalSort.TopologicalSort<>.<EdgesWalkLazy>d__35))]
        protected System.Collections.Generic.IEnumerable<int> EdgesWalkLazy(int node, System.Collections.Generic.List<System.Collections.Generic.HashSet<int>> edgesWalk) { }
        public System.Collections.Generic.IList<System.Collections.Generic.IEnumerable<T>> GetConflicts() { }
        public System.Collections.Generic.IEnumerable<T> GetDependents(T node, bool immediate = False, bool terminating = False) { }
        protected System.Collections.Generic.IEnumerable<int> GetDependents(int node, bool immediate, bool terminating) { }
        public System.Collections.Generic.IEnumerable<T> GetPrecedents(T node, bool immediate = False, bool terminating = False) { }
        protected System.Collections.Generic.IEnumerable<int> GetPrecedents(int node, bool immediate, bool terminating) { }
        protected System.Collections.Generic.IEnumerable<int> GetRelated(int node, bool immediate, bool terminating, System.Collections.Generic.List<System.Collections.Generic.HashSet<int>> edgesData, System.Collections.Generic.List<System.Collections.Generic.HashSet<int>> transData = null, bool lazy = False) { }
        protected virtual int NodeKey(T node) { }
        protected int NodeKeySafe(T node, System.Collections.Generic.Dictionary<T, int> tempDict) { }
        public System.Collections.Generic.IList<T> Sort() { }
    }
}
namespace System.Linq
{
    
    public class static TimSortExtender { }
}