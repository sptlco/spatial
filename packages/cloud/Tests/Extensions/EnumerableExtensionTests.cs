// Copyright © Spatial Corporation. All rights reserved.

namespace Spatial.Extensions;

/// <summary>
/// Tests for <see cref="EnumerableExtensions"/>.
/// </summary>
public class EnumerableExtensionTests
{
    /// <summary>
    /// Convert the <see cref="IEnumerable{T}"/> to an array.
    /// </summary>
    [Fact]
    [Trait("Category", "Unit")]
    public void TestToArray()
    {
        var enumerable = new List<int> { 1, 2, 3 };

        var result = enumerable.ToArray(x => x.ToString());

        Assert.Equal(["1", "2", "3"], result);
    }
}