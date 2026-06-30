// Copyright © Spatial Corporation. All rights reserved.

namespace Spatial.Extensions;

/// <summary>
/// Tests for <see cref="DelegateExtensions"/>.
/// </summary>
public class DelegateExtensionTests
{
    private static int Add(int a, int b) => a + b;

    private static int Subtract(int a, int b) => a - b;

    private static string Describe(int value) => value.ToString();

    /// <summary>
    /// Get a unique <see cref="Delegate"/> identifier.
    /// </summary>
    [Fact]
    [Trait("Category", "Unit")]
    public void TestGetOrCreateIdentifier()
    {
        Func<int, int, int> add = Add;

        var identifier = add.GetOrCreateIdentifier();

        Assert.Equal("System.Int32 Spatial.Extensions.DelegateExtensionTests.Add(System.Int32,System.Int32)", identifier);
    }

    /// <summary>
    /// Different methods produce different identifiers.
    /// </summary>
    [Fact]
    [Trait("Category", "Unit")]
    public void TestGetOrCreateIdentifierDiffersForDifferentMethods()
    {
        Func<int, int, int> add = Add;
        Func<int, int, int> subtract = Subtract;

        Assert.NotEqual(add.GetOrCreateIdentifier(), subtract.GetOrCreateIdentifier());
    }

    /// <summary>
    /// Identifiers differ based on parameter and return types.
    /// </summary>
    [Fact]
    [Trait("Category", "Unit")]
    public void TestGetOrCreateIdentifierReflectsSignature()
    {
        Func<int, string> describe = Describe;

        var identifier = describe.GetOrCreateIdentifier();

        Assert.Equal("System.String Spatial.Extensions.DelegateExtensionTests.Describe(System.Int32)", identifier);
    }

    /// <summary>
    /// The same method produces a cached, equal identifier across calls.
    /// </summary>
    [Fact]
    [Trait("Category", "Unit")]
    public void TestGetOrCreateIdentifierIsCached()
    {
        Func<int, int, int> first = Add;
        Func<int, int, int> second = Add;

        Assert.Equal(first.GetOrCreateIdentifier(), second.GetOrCreateIdentifier());
        Assert.Same(first.GetOrCreateIdentifier(), second.GetOrCreateIdentifier());
    }
}