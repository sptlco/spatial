// Copyright © Spatial Corporation. All rights reserved.

namespace Spatial.Structures;

/// <summary>
/// Tests for <see cref="ConcurrentHashSet{T}"/>.
/// </summary>
public class ConcurrentHashSetTests
{
    /// <summary>
    /// Create an empty <see cref="ConcurrentHashSet{T}"/>.
    /// </summary>
    [Fact]
    [Trait("Category", "Unit")]
    public void TestConstructorEmpty()
    {
        var set = new ConcurrentHashSet<int>();

        Assert.Equal(0, set.Count);
    }

    /// <summary>
    /// Create a <see cref="ConcurrentHashSet{T}"/> from a collection.
    /// </summary>
    [Fact]
    [Trait("Category", "Unit")]
    public void TestConstructorFromCollection()
    {
        var set = new ConcurrentHashSet<int>([1, 2, 3]);

        Assert.Equal(3, set.Count);
        Assert.True(set.Contains(1));
        Assert.True(set.Contains(2));
        Assert.True(set.Contains(3));
    }

    /// <summary>
    /// Create a <see cref="ConcurrentHashSet{T}"/> from a collection with duplicate values.
    /// </summary>
    [Fact]
    [Trait("Category", "Unit")]
    public void TestConstructorFromCollectionWithDuplicates()
    {
        var set = new ConcurrentHashSet<int>([1, 1, 2]);

        Assert.Equal(2, set.Count);
    }

    /// <summary>
    /// Add a value to the <see cref="ConcurrentHashSet{T}"/>.
    /// </summary>
    [Fact]
    [Trait("Category", "Unit")]
    public void TestAdd()
    {
        var set = new ConcurrentHashSet<int>();

        set.Add(1);

        Assert.Equal(1, set.Count);
        Assert.True(set.Contains(1));
    }

    /// <summary>
    /// Add a value that already exists in the <see cref="ConcurrentHashSet{T}"/>.
    /// </summary>
    [Fact]
    [Trait("Category", "Unit")]
    public void TestAddDuplicate()
    {
        var set = new ConcurrentHashSet<int>();

        set.Add(1);
        set.Add(1);

        Assert.Equal(1, set.Count);
    }

    /// <summary>
    /// Remove a value from the <see cref="ConcurrentHashSet{T}"/>.
    /// </summary>
    [Fact]
    [Trait("Category", "Unit")]
    public void TestRemove()
    {
        var set = new ConcurrentHashSet<int>([1, 2, 3]);

        set.Remove(2);

        Assert.Equal(2, set.Count);
        Assert.False(set.Contains(2));
    }

    /// <summary>
    /// Remove a value that does not exist in the <see cref="ConcurrentHashSet{T}"/>.
    /// </summary>
    [Fact]
    [Trait("Category", "Unit")]
    public void TestRemoveMissingValue()
    {
        var set = new ConcurrentHashSet<int>([1, 2, 3]);

        set.Remove(4);

        Assert.Equal(3, set.Count);
    }

    /// <summary>
    /// Get whether or not the <see cref="ConcurrentHashSet{T}"/> contains a value.
    /// </summary>
    [Fact]
    [Trait("Category", "Unit")]
    public void TestContains()
    {
        var set = new ConcurrentHashSet<int>([1, 2, 3]);

        Assert.True(set.Contains(1));
        Assert.False(set.Contains(4));
    }

    /// <summary>
    /// Clear the <see cref="ConcurrentHashSet{T}"/>.
    /// </summary>
    [Fact]
    [Trait("Category", "Unit")]
    public void TestClear()
    {
        var set = new ConcurrentHashSet<int>([1, 2, 3]);

        set.Clear();

        Assert.Equal(0, set.Count);
    }

    /// <summary>
    /// Enumerate the <see cref="ConcurrentHashSet{T}"/> using the generic enumerator.
    /// </summary>
    [Fact]
    [Trait("Category", "Unit")]
    public void TestGetEnumerator()
    {
        var set = new ConcurrentHashSet<int>([1, 2, 3]);

        var values = set.ToList();

        Assert.Equal([1, 2, 3], values.OrderBy(v => v));
    }

    /// <summary>
    /// Enumerate the <see cref="ConcurrentHashSet{T}"/> using the non-generic enumerator.
    /// </summary>
    [Fact]
    [Trait("Category", "Unit")]
    public void TestGetEnumeratorNonGeneric()
    {
        var set = new ConcurrentHashSet<int>([1, 2, 3]);
        var enumerable = (System.Collections.IEnumerable) set;

        var values = new List<int>();

        foreach (var value in enumerable)
        {
            values.Add((int) value);
        }

        Assert.Equal([1, 2, 3], values.OrderBy(v => v));
    }
}