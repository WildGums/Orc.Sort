namespace Orc.Algorithms.Sort
{
    using System.Collections.Generic;

    using Orc.Algorithms.Sort.Interfaces;

    public class QuickSortBaseClass<T> : ISorter<T>
    {
        public void Sort(IList<T> list)
        {
            ((List<T>) list).Sort();
        }
    }
}
