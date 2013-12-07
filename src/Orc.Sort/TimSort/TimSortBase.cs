#region Licence

/*
 * Copyright (C) 2008 The Android Open Source Project
 *
 * Licensed under the Apache License, Version 2.0 (the "License");
 * you may not use this file except in compliance with the License.
 * You may obtain a copy of the License at
 *
 *      http://www.apache.org/licenses/LICENSE-2.0
 *
 * Unless required by applicable law or agreed to in writing, software
 * distributed under the License is distributed on an "AS IS" BASIS,
 * WITHOUT WARRANTIES OR CONDITIONS OF ANY KIND, either express or implied.
 * See the License for the specific language governing permissions and
 * limitations under the License.
 */

#endregion

#region Notes
//------------------------------------------------------------------------------
// Java implementation:
//
// A stable, adaptive, iterative mergesort that requires far fewer than
// n lg(n) comparisons when running on partially sorted arrays, while
// offering performance comparable to a traditional mergesort when run
// on random arrays.  Like all proper mergesorts, this sort is stable and
// runs O(n log n) time (worst case).  In the worst case, this sort requires
// temporary storage space for n/2 object references; in the best case,
// it requires only a small constant amount of space.
// 
// This implementation was adapted from Tim Peters's list sort for
// Python, which is described in detail here:
// http://svn.python.org/projects/python/trunk/Objects/listsort.txt
// 
// Tim's C code may be found here:
// http://svn.python.org/projects/python/trunk/Objects/listobject.c
// 
// The underlying techniques are described in this paper (and may have
// even earlier origins):
// 
// "Optimistic Sorting and Information Theoretic Complexity"
// Peter McIlroy
// SODA (Fourth Annual ACM-SIAM Symposium on Discrete Algorithms),
// pp 467-474, Austin, Texas, 25-27 January 1993.
// 
// While the API to this class consists solely of static methods, it is
// (privately) instantiable; a TimSort instance holds the state of an ongoing
// sort, assuming the input array is large enough to warrant the full-blown
// TimSort. Small arrays are sorted in place, using a binary insertion sort.
// 
// author: Josh Bloch
//------------------------------------------------------------------------------
// C# implementation:
//
// This implementation was adapted from Josh Bloch array sort for Java, 
// which has been found here:
// http://gee.cs.oswego.edu/cgi-bin/viewcvs.cgi/jsr166/src/main/java/util/TimSort.java?view=co
// 
// author: Milosz Krajewski
//------------------------------------------------------------------------------
#endregion

using System;
using System.Diagnostics;

namespace TimSort
{
	#region class TimSortBase<TList, TItem>

	/// <summary>
	/// TimSortBase is base class for all TimSort specific classes. It contains common methods.
	/// Should not be used directly.
	/// </summary>
	/// <typeparam name="TList">The type of the list.</typeparam>
	/// <typeparam name="TItem">The type of the item.</typeparam>
	internal abstract class TimSortBase<TList, TItem>
	{
		#region consts

		/// <summary>
		/// This is the minimum sized sequence that will be merged.  Shorter
		/// sequences will be lengthened by calling BinarySort.  If the entire
		/// array is less than this length, no merges will be performed.
		/// This constant should be a power of two.  It was 64 in Tim Peter's C
		/// implementation, but 32 was empirically determined to work better in
		/// this implementation.  In the unlikely event that you set this constant
		/// to be a number that's not a power of two, you'll need to change the
        /// <c>minRunLength</c> computation.
		/// If you decrease this constant, you must change the stackLen
		/// computation in the TimSort constructor, or you risk an
		/// ArrayOutOfBounds exception.  See listsort.txt for a discussion
		/// of the minimum stack length required as a function of the length
		/// of the array being sorted and the minimum merge sequence length.
		/// </summary>
		protected const int MIN_MERGE = 32;

		/// <summary>
		/// When we get into galloping mode, we stay there until both runs win less
		/// often than MIN_GALLOP consecutive times.
		/// </summary>
		protected const int MIN_GALLOP = 7;

		/// <summary>
		/// Maximum initial size of tmp array, which is used for merging. 
		/// The array can grow to accommodate demand.
		/// Unlike Tim's original C version, we do not allocate this much storage
		/// when sorting smaller arrays. This change was required for performance.
		/// </summary>
		protected const int INITIAL_TMP_STORAGE_LENGTH = 256;

		#endregion

		#region fields

		/// <summary>The array being sorted.</summary>
		protected readonly TList _array;

		/// <summary>Cached length of array, it won't change.</summary>
		protected readonly int _arrayLength;

		/// <summary>
		/// This controls when we get *into* galloping mode.  It is initialized
		/// to MIN_GALLOP.  The mergeLo and mergeHi methods nudge it higher for
		/// random data, and lower for highly structured data.
		/// </summary>
		protected int _minGallop = MIN_GALLOP;

		/// <summary>
		/// Temp storage for merges.
		/// </summary>
		protected TItem[] _mergeBuffer;

		/// <summary>
		/// A stack of pending runs yet to be merged.  Run i starts at
		/// address base[i] and extends for len[i] elements.  It's always
		/// true (so long as the indices are in bounds) that:
		/// <c>runBase[i] + runLen[i] == runBase[i + 1]</c>
		/// so we could cut the storage for this, but it's a minor amount,
		/// and keeping all the info explicit simplifies the code.
		/// </summary>
		protected int _stackSize; // = 0; // Number of pending runs on stack
		protected int[] _runBase;
		protected int[] _runLength;

		#endregion

		#region constructor

		/// <summary>Initializes a new instance of the <see cref="TimSortBase&lt;TList, TItem&gt;"/> class.</summary>
		/// <param name="array">The array.</param>
		/// <param name="arrayLength">Length of the array.</param>
		protected TimSortBase(TList array, int arrayLength)
		{
			_array = array;
			_arrayLength = arrayLength;

			// Allocate temp storage (which may be increased later if necessary)
			int mergeBufferLength =
				arrayLength < 2 * INITIAL_TMP_STORAGE_LENGTH
				? arrayLength >> 1
				: INITIAL_TMP_STORAGE_LENGTH;
			_mergeBuffer = new TItem[mergeBufferLength];

			// Allocate runs-to-be-merged stack (which cannot be expanded).  The
			// stack length requirements are described in listsort.txt.  The C
			// version always uses the same stack length (85), but this was
			// measured to be too expensive when sorting "mid-sized" arrays (e.g.,
			// 100 elements) in Java.  Therefore, we use smaller (but sufficiently
			// large) stack lengths for smaller arrays.  The "magic numbers" in the
			// computation below must be changed if MIN_MERGE is decreased.  See
			// the MIN_MERGE declaration above for more information.
			int stackLength =
				arrayLength < 120 ? 5 :
				arrayLength < 1542 ? 10 :
				arrayLength < 119151 ? 19 :
				40;
			_runBase = new int[stackLength];
			_runLength = new int[stackLength];
		}

		#endregion

		#region protected interface

		/// <summary>
		/// Returns the minimum acceptable run length for an array of the specified length. Natural runs shorter than this 
		/// will be extended with BinarySort.
		/// Roughly speaking, the computation is:
		/// If <c>n &lt; MIN_MERGE</c>, return n (it's too small to bother with fancy stuff).
		/// Else if n is an exact power of 2, return <c>MIN_MERGE/2</c>.
		/// Else return an int k, <c>MIN_MERGE/2 &lt;= k &lt;= MIN_MERGE</c>, such that <c>n/k</c> is close to, but strictly 
		/// less than, an exact power of 2. For the rationale, see listsort.txt.
		/// </summary>
		/// <param name="n">the length of the array to be sorted.</param>
		/// <returns>the length of the minimum run to be merged.</returns>
		protected static int GetMinimumRunLength(int n)
		{
			Debug.Assert(n >= 0);
			int r = 0; // Becomes 1 if any 1 bits are shifted off
			while (n >= MIN_MERGE)
			{
				r |= (n & 1);
				n >>= 1;
			}
			return n + r;
		}

		/// <summary>
		/// Merges the two runs at stack indices i and i+1. 
		/// Run i must be the penultimate or antepenultimate run on the stack. 
		/// In other words, i must be equal to stackSize-2 or stackSize-3.
		/// </summary>
		/// <param name="runIndex">stack index of the first of the two runs to merge.</param>
		protected abstract void MergeAt(int runIndex);

		/// <summary>
		/// Examines the stack of runs waiting to be merged and merges adjacent runs until the stack invariants are
		/// reestablished: 
		/// <c><![CDATA[1. runLen[i - 3] > runLen[i - 2] + runLen[i - 1] ]]></c> and 
		/// <c><![CDATA[2. runLen[i - 2] > runLen[i - 1] ]]></c>
		/// This method is called each time a new run is pushed onto the stack,
		/// so the invariants are guaranteed to hold for i &lt; stackSize upon
		/// entry to the method.
		/// </summary>
		protected void MergeCollapse()
		{
			while (_stackSize > 1)
			{
				var n = _stackSize - 2;

				if (n > 0 && _runLength[n - 1] <= _runLength[n] + _runLength[n + 1])
				{
					if (_runLength[n - 1] < _runLength[n + 1]) n--;
					MergeAt(n);
				}
				else if (_runLength[n] <= _runLength[n + 1])
				{
					MergeAt(n);
				}
				else
				{
					break; // Invariant is established
				}
			}
		}

		/// <summary>
		/// Merges all runs on the stack until only one remains.  This method is called once, to complete the sort.
		/// </summary>
		protected void MergeForceCollapse()
		{
			while (_stackSize > 1)
			{
				var n = _stackSize - 2;
				if (n > 0 && _runLength[n - 1] < _runLength[n + 1]) n--;
				MergeAt(n);
			}
		}

		/// <summary>
		/// Pushes the specified run onto the pending-run stack.
		/// </summary>
		/// <param name="runBase">index of the first element in the run.</param>
		/// <param name="runLength">the number of elements in the run.</param>
		protected void PushRun(int runBase, int runLength)
		{
			_runBase[_stackSize] = runBase;
			_runLength[_stackSize] = runLength;
			_stackSize++;
		}

		/// <summary>
		/// Ensures that the external array tmp has at least the specified
		/// number of elements, increasing its size if necessary.  The size
		/// increases exponentially to ensure amortized linear time complexity.
		/// </summary>
		/// <param name="minCapacity">the minimum required capacity of the tmp array.</param>
		/// <returns>tmp, whether or not it grew</returns>
		protected TItem[] EnsureCapacity(int minCapacity)
		{
			if (_mergeBuffer.Length < minCapacity)
			{
				// Compute smallest power of 2 > minCapacity
				int newSize = minCapacity;
				newSize |= newSize >> 1;
				newSize |= newSize >> 2;
				newSize |= newSize >> 4;
				newSize |= newSize >> 8;
				newSize |= newSize >> 16;
				newSize++;

				newSize = newSize < 0 ? minCapacity : Math.Min(newSize, _arrayLength >> 1);

				_mergeBuffer = new TItem[newSize];
			}
			return _mergeBuffer;
		}

		/// <summary>
		/// Checks that fromIndex and toIndex are in range, and throws an
		/// appropriate exception if they aren't.
		/// </summary>
		/// <param name="arrayLen">the length of the array.</param>
		/// <param name="fromIndex">the index of the first element of the range.</param>
		/// <param name="toIndex">the index after the last element of the range.</param>
		protected static void CheckRange(int arrayLen, int fromIndex, int toIndex)
		{
			if (fromIndex > toIndex)
				throw new ArgumentException(string.Format("fromIndex({0}) > toIndex({1})", fromIndex, toIndex));
			if (fromIndex < 0)
				throw new IndexOutOfRangeException(string.Format("fromIndex ({0}) is out of bounds", fromIndex));
			if (toIndex > arrayLen)
				throw new IndexOutOfRangeException(string.Format("toIndex ({0}) is out of bounds", toIndex));
		}

		/// <summary>Reverse the specified range of the specified array.</summary>
		/// <param name="array">the array in which a range is to be reversed.</param>
		/// <param name="lo">the index of the first element in the range to be reversed.</param>
		/// <param name="hi">the index after the last element in the range to be reversed.</param>
		protected static void ArrayReverseRange(TItem[] array, int lo, int hi)
		{
			Array.Reverse(array, lo, hi - lo);
		}

        /// <summary>Copies specified array range.</summary>
        /// <param name="buffer">The buffer.</param>
        /// <param name="sourceIndex">Index of the source.</param>
        /// <param name="targetIndex">Index of the target.</param>
        /// <param name="length">The length.</param>
        protected static void ArrayCopyRange(TItem[] buffer, int sourceIndex, int targetIndex, int length)
        {
            Array.Copy(buffer, sourceIndex, buffer, targetIndex, length);
        }

        /// <summary>Copies specified array range.</summary>
        /// <param name="source">The source.</param>
        /// <param name="sourceIndex">Index of the source.</param>
        /// <param name="target">The target.</param>
        /// <param name="targetIndex">Index of the target.</param>
        /// <param name="length">The length.</param>
        protected static void ArrayCopyRange(TItem[] source, int sourceIndex, TItem[] target, int targetIndex, int length)
        {
            Array.Copy(source, sourceIndex, target, targetIndex, length);
        }

		#endregion
	}

	#endregion
}
