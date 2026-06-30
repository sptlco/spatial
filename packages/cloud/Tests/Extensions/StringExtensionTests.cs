// Copyright © Spatial Corporation. All rights reserved.

namespace Spatial.Extensions;

/// <summary>
/// Tests for <see cref="StringExtensions"/>.
/// </summary>
public class StringExtensionTests
{
    /// <summary>
    /// Get whether or not a string matches a wildcard value.
    /// </summary>
    [Theory]
    [Trait("Category", "Unit")]
    [InlineData("hello.txt", "*.txt", true)]
    [InlineData("hello.txt", "*.csv", false)]
    [InlineData("hello.txt", "h?llo.txt", true)]
    [InlineData("hello.txt", "h?llo.csv", false)]
    [InlineData("hello.txt", "hello.txt", true)]
    public void TestMatches(string value, string wildcard, bool expected)
    {
        Assert.Equal(expected, value.Matches(wildcard));
    }

    /// <summary>
    /// Normalize a path string.
    /// </summary>
    [Fact]
    [Trait("Category", "Unit")]
    public void TestToNormalizedPath()
    {
        Assert.Equal("a\\b\\c", "a/b/c".ToNormalizedPath());
    }

    /// <summary>
    /// Convert a C-style string format to a positional string format.
    /// </summary>
    [Fact]
    [Trait("Category", "Unit")]
    public void TestToPositionalFormat()
    {
        Assert.Equal("{0} is {1} years old", "%s is %d years old".ToPositionalFormat());
    }

    /// <summary>
    /// Convert a C-style string format with a literal percent sign to a positional string format.
    /// </summary>
    [Fact]
    [Trait("Category", "Unit")]
    public void TestToPositionalFormatPreservesLiteralPercent()
    {
        Assert.Equal("{0}% complete", "%d%% complete".ToPositionalFormat());
    }
}