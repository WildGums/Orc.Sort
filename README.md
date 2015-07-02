# Orc.Sort

[![Join the chat at https://gitter.im/WildGums/Orc.Sort](https://badges.gitter.im/Join%20Chat.svg)](https://gitter.im/WildGums/Orc.Sort?utm_source=badge&utm_medium=badge&utm_campaign=pr-badge&utm_content=badge)

![License](https://img.shields.io/github/license/wildgums/orc.sort.svg)
![NuGet downloads](https://img.shields.io/nuget/dt/orc.sort.svg)
![Version](https://img.shields.io/nuget/v/orc.sort.svg)
![Pre-release version](https://img.shields.io/nuget/vpre/orc.sort.svg)

This library contains various sorting Algorithms.


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

Information on Topological Sort can be found here: http://en.wikipedia.org/wiki/Topological_sorting

**NOTE:**

As shown in the example on the wikipedia page, it is important to realise a DAG may have multiple valid solutions.


## PriorityTopologicalSort

Similar to TopologicalSort, except the order in which dependency lists are added to the sort algorithm is important.


*Example:*

	DependencyLists:
	- (1) A, C
	- (2) B, C
	

A valid topological result would be "B, A, C".

However the priority topological sort will return "A, B, C" because "A" appeared in a dependency list before "B".

Please see the unit tests for more examples.

**NOTE:**

The dependency structure is expressed by the ordering of the elements in a list. In the previous example "A, C" means "A comes before C".

If we had "A, B, C, D", this would mean "A comes before B, which comes before C, which comes before D" or in other words "D depends on C, which depends on B, which depends on A".



## TemplateSort

This sort algorithm will sort a list based on the values of another list.

*Example:*

    SourceCollection = C, B, B, L, N, P, C, A, D, E, B, E
    TemplateCollection = A, B, C, D

	SourceCollection.SortAccordingTo(TemplateCollection)
    
    Result = A, B, B, B, C, C, D, L, N, P, E, E

If there are items in the list to be sorted that are not in the template list, they simply get appended to the end of the result in the order they are found.

### Features
- TemplateCollection can be a complex type in which case SortAccordingTo() method will accept a lambda expression to sort by a specific property

    `SourceCollection.SortAccordingTo(TemplateCollection, x => x.SomeProperty)`

- You can choose whether to append items that did not have a match at the end of the result or not.
- Implement your own IEqualityComparer

	`SourceCollection.SortAccordingTo(TemplateCollection, comparer: StringComparer.CurrentCultureIgnoreCase);`

## Roadmap

- Add randomising of collection (total opposite to sorting :)
