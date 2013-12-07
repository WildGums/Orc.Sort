namespace Orc.Sort.NSort
{
    /// <summary>
	/// Shear sort parralel algorithm
	/// </summary>
	/// <remarks>
	/// <para>
	/// Source: <a href="http://www.cs.rit.edu/~atk/Java/Sorting/sorting.html">
	/// http://www.cs.rit.edu/~atk/Java/Sorting/sorting.html</a>
	/// </para>
	/// </remarks>
	public class ShearSorter // : SwapSorter
	{
        //private int cols;
        //private int log;

        //private int rows;

        //public ShearSorter()
        //    :base()
        //{
        //}

        //public ShearSorter(IComparer comparer, ISwap swapper)
        //    :base(comparer,swapper)
        //{
        //}

        //public override void Sort(IList list)
        //{
        //    int pow=1, div=1;

        //    for(int i=1; i*i<=list.Count; ++i)
        //    {
        //        if (list.Count % i == 0)
        //            div = i;
        //    }

        //    this.rows = div; 
        //    this.cols = list.Count / div;
        //    for(this.log=0; pow<=this.rows; ++this.log) 
        //        pow = pow * 2;

        //    int[] h = new int[this.rows];
        //    for (int i=0; i<this.rows; ++i)
        //        h[i]=i*this.cols;

        //    for (int k=0; k<this.log; ++k) 
        //    {
        //        for (int j=0; j<this.cols/2; ++j) 
        //        {
        //            for (int i=0; i<this.rows; i++)
        //                this.SortPart1(list,i*this.cols,(i+1)*this.cols,1,(i%2==0?true:false));
        //            for (int i=0; i<this.rows; i++)
        //                this.SortPart2(list,i*this.cols,(i+1)*this.cols,1,(i%2==0?true:false));
        //        }
        //        for (int j=0; j<this.rows/2; j++) 
        //        {
        //            for (int i=0; i<this.cols; i++)
        //                this.SortPart1(list,i,this.rows*this.cols+i,this.cols,true);
        //            for (int i=0; i<this.cols; i++)
        //                this.SortPart2(list,i,this.rows*this.cols+i,this.cols,true);
        //        }
        //    }
        //    for (int j=0; j<this.cols/2; j++) 
        //    {
        //        for (int i=0; i<this.rows; i++)
        //            this.SortPart1(list,i*this.cols,(i+1)*this.cols,1,true);
        //        for (int i=0; i<this.rows; i++)
        //            this.SortPart2(list,i*this.cols,(i+1)*this.cols,1,true);
        //    }
        //    for (int i=0; i<this.rows; i++)
        //        h[i]=-1;
        //}

        //internal void SortPart1(IList list, int Lo, int Hi, int Nx, bool Up)
        //{
        //    int c;
        //    for (int j = Lo; j+Nx<Hi; j+=2*Nx) 
        //    {
        //        c = this.Comparer.Compare(list[j],list[j+Nx]);
        //        if ((Up &&  c > 0) || !Up && c < 0) 
        //            this.Swapper.Swap(list,j,j+Nx);
        //    }
        //}

        //internal void SortPart2(IList list, int Lo, int Hi, int Nx, bool Up)
        //{
        //    int c;
        //    for (int j = Lo+Nx; j+Nx<Hi; j+=2*Nx) 
        //    {
        //        c = this.Comparer.Compare(list[j],list[j+Nx]);
        //        if ((Up && c > 0) || !Up && c < 0) 
        //            this.Swapper.Swap(list,j,j+Nx);
        //    }
        //}

	}
}
