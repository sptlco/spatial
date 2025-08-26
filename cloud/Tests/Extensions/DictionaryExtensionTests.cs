// Copyright © Spatial Corporation. All rights reserved.

namespace Spatial.Extensions;

/// <summary>
/// Unit tests for <see cref="DictionaryExtensions"/>.
/// </summary>
public class DictionaryExtensionTests
{
    /// <summary>
    /// Test <see cref="DictionaryExtensions.GetOrAdd"/>
    /// </summary>
    [Fact]
    public void TestGetOrAdd()
    {
        var dict = new Dictionary<int, string>();
        var value = dict.GetOrAdd(1, num => $"I am number {num}!");

        Assert.Equal("I am number 1!", value);
    }
}
