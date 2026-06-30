// Copyright © Spatial Corporation. All rights reserved.

namespace Spatial.Extensions;

/// <summary>
/// Tests for <see cref="CollectionExtensions"/>.
/// </summary>
public class CollectionExtensionTests
{
    /// <summary>
    /// Convert a collection to a padded array.
    /// </summary>
    [Fact]
    [Trait("Category", "Unit")]
    public void TestToPaddedArray()
    {
        var collection = new List<int> { 1, 2, 3 };

        var result = collection.ToPaddedArray(5, x => x.ToString(), "pad");

        Assert.Equal(["1", "2", "3", "pad", "pad"], result);
    }

    /// <summary>
    /// Execute an action for each element of a collection.
    /// </summary>
    [Fact]
    [Trait("Category", "Unit")]
    public void TestForEach()
    {
        var collection = new List<int> { 1, 2, 3 };
        var sum = 0;

        collection.ForEach(x => sum += x);

        Assert.Equal(6, sum);
    }

    /// <summary>
    /// Filter a collection.
    /// </summary>
    [Fact]
    [Trait("Category", "Unit")]
    public void TestFilter()
    {
        var collection = new List<int> { 1, 2, 3, 4, 5 };

        var result = collection.Filter(x => x % 2 == 0);

        Assert.Equal([2, 4], result);
    }

    /// <summary>
    /// Map the elements of a collection to a list.
    /// </summary>
    [Fact]
    [Trait("Category", "Unit")]
    public void TestMap()
    {
        var collection = new List<int> { 1, 2, 3 };

        var result = collection.Map(x => x * 2);

        Assert.Equal([2, 4, 6], result);
    }
}