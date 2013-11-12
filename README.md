# Orc.Sort

This library has a few efficient sorting Algorithms.


## TimSort

Is taken from https://timsort4net.codeplex.com/

Documentation can be found on the original site.

We have found TimSort to be significantly faster than the inbuilt QuickSort implementation on generally ordered data (either ascending or descending). It is also nearly as fast on random data.

## NSort

Is taken from http://www.codeproject.com/Articles/6033/Sorting-Algorithms-In-C?fid=32969&fr=26#xx0xx

Documentation can be found on the original site.

A generic implementation is also included in this library. 

These sorting algorithms are more for academic purposes. There are a lot of different types of sorting algorithms implemented, that may come in useful for specific scenarios.

## TopologicalSort

## PriorityTopologicalSort

Similar to TopologicalSort, except the order in which dependency lists are added to the sort algorithm is important.

Example

	DependencyLists:
	- (1) A, C
	- (2) B, C
	

A valid topological result would be "B, A, C".

However the priority topological sort will return "A, B, C" because "A" appeared in a dependency list before "B".

Please see the unit tests for more examples.


## TemplateSort

This sort algorithm will sort a list based on the values of another list.

Example:

    ListToSort = C, B, B, L, N, P, C, A, D, E, B, E
    TemplateList = A, B, C, D

	ListToSort.SortAccordingTo(TemplateList)
    
    Result = A, B, B, B, C, C, D, L, N, P, E, E

If there are items in the list to be sorted that are not in the template list, then simply get appended to the end of the result in the order they are found.