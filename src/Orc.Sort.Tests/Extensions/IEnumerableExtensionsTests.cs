﻿namespace Orc.Sort.Tests;

using System;
using System.Collections.Generic;
using System.Linq;
using NUnit.Framework;

[TestFixture]
public class IEnumerableExtensionsTests
{
    [Test]
    public void MergeSorted_ReturnsCorrectSequence()
    {
        var list_1 = new List<Item>
        {
            new Item("B", 1), new Item("C", 1), new Item("F", 1), new Item("H", 1),
        };

        var list_2 = new List<Item>
        {
            new Item("B", 2), new Item("D", 2), new Item("E", 2), new Item("G", 2),
        };

        var list_3 = new List<Item>
        {
            new Item("A", 3), new Item("B", 3), new Item("E", 3), new Item("H", 3),
        };

        var sorted = new List<Item>
        {
            new Item("A", 3), new Item("B", 1), new Item("B", 2), new Item("B", 3),
            new Item("C", 1), new Item("D", 2), new Item("E", 2), new Item("E", 3),
            new Item("F", 1), new Item("G", 2), new Item("H", 1), new Item("H", 3),
        };
        sorted.Sort();

        var result = (new List<IEnumerable<Item>> {list_1, list_2, list_3}).MergeSorted().ToList();

        Assert.AreEqual(sorted, result);
    }

    [Test]
    public void MergeSorted_ReturnsCorrectDistinctSequence()
    {
        var list_1 = new List<Item>
        {
            new Item("B", 1), new Item("C", 1), new Item("F", 1), new Item("H", 1),
        };

        var list_2 = new List<Item>
        {
            new Item("B", 2), new Item("D", 2), new Item("E", 2), new Item("G", 2),
        };

        var list_3 = new List<Item>
        {
            new Item("A", 3), new Item("B", 3), new Item("E", 3), new Item("H", 3),
        };

        var sorted = new List<Item>
        {
            new Item("A", 3), new Item("B", 1),
            new Item("C", 1), new Item("D", 2), new Item("E", 2),
            new Item("F", 1), new Item("G", 2), new Item("H", 1),
        };
        sorted.Sort();

        var result = (new List<IEnumerable<Item>> {list_1, list_2, list_3}).MergeSorted(true).ToList();

        Assert.AreEqual(sorted, result);
    }

    [Test]
    public void MergeSortedMany_ReturnsCorrectSequence()
    {
        var list_1 = new List<Item>
        {
            new Item("B", 1), new Item("C", 1), new Item("F", 1), new Item("H", 1),
        };

        var list_2 = new List<Item>
        {
            new Item("B", 2), new Item("D", 2), new Item("E", 2), new Item("G", 2),
        };

        var list_3 = new List<Item>
        {
            new Item("A", 3), new Item("B", 3), new Item("E", 3), new Item("H", 3),
        };

        var sorted = new List<Item>
        {
            new Item("A", 3), new Item("B", 1), new Item("B", 2), new Item("B", 3),
            new Item("C", 1), new Item("D", 2), new Item("E", 2), new Item("E", 3),
            new Item("F", 1), new Item("G", 2), new Item("H", 1), new Item("H", 3),
        };
        sorted.Sort();

        var comparer = Comparer<Item>.Default;
        var result = (new List<IEnumerable<Item>> {list_1, list_2, list_3}).MergeSortedMany(comparer).ToList();

        Assert.AreEqual(sorted, result);
    }
}

internal class Item : IComparable<Item>, IEquatable<Item>
{
    public Item(string key, int listID)
    {
        Key = key;
        ListID = listID;
    }
    public int ListID { get; }
    public string Key { get; }

    public int CompareTo(Item other)
    {
        var result = string.Compare(Key, other.Key, StringComparison.Ordinal);

        return result;
    }

    public bool Equals(Item other)
    {
        return Key.Equals(other.Key);
    }

    public override string ToString()
    {
        return $"{Key},{ListID}";
    }
}
