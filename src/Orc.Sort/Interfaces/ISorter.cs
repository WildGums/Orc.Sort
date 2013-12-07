namespace Orc.Sort.Interfaces
{
    using System.Collections;
    using System.Collections.Generic;

    /// <summary>
	/// Summary description for ISorter.
	/// </summary>
	public interface ISorter
	{
		void Sort(IList list);
	}

    public interface ISorter<T>
    {
        void Sort(IList<T> list);
    }
}
