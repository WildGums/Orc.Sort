namespace Orc.Sort
{
    using System.Collections;
    using System.Collections.Generic;

    /// <summary>
    /// </summary>
    /// Object swapper interface
    public interface ISwap
    {
        void Swap(IList array, int left, int right);
        void Set(IList array, int left, int right);
        void Set(IList array, int left, object? obj);
    }

    public interface ISwap<T>
    {
        void Swap(IList<T> array, int left, int right);
        void Set(IList<T> array, int left, int right);
        void Set(IList<T> array, int left, T obj);
    }
}
