// Copyright © Spatial Corporation. All rights reserved.

namespace Spatial.Extensions;

/// <summary>
/// Tests for <see cref="DictionaryExtensions"/>.
/// </summary>
public class DictionaryExtensionTests
{
    /// <summary>
    /// Get an existing value without invoking the factory.
    /// </summary>
    [Fact]
    [Trait("Category", "Unit")]
    public void TestGetOrAddReturnsExistingValue()
    {
        var dictionary = new Dictionary<string, int> { ["a"] = 1 };

        var result = dictionary.GetOrAdd("a", _ => throw new InvalidOperationException("Factory should not be called."));

        Assert.Equal(1, result);
    }

    /// <summary>
    /// Add a value using the factory if the key doesn't exist.
    /// </summary>
    [Fact]
    [Trait("Category", "Unit")]
    public void TestGetOrAddAddsMissingValue()
    {
        var dictionary = new Dictionary<string, int>();

        var result = dictionary.GetOrAdd("a", _ => 42);

        Assert.Equal(42, result);
        Assert.Equal(42, dictionary["a"]);
    }

    /// <summary>
    /// Process all elements in the dictionary.
    /// </summary>
    [Fact]
    [Trait("Category", "Unit")]
    public void TestForEach()
    {
        var dictionary = new Dictionary<string, int> { ["a"] = 1, ["b"] = 2, ["c"] = 3 };
        var sum = 0;
        var keys = new List<string>();

        dictionary.ForEach((key, value) => {
            keys.Add(key);
            sum += value;
        });

        Assert.Equal(6, sum);
        Assert.Equal(["a", "b", "c"], keys);
    }
}