// Copyright © Spatial Corporation. All rights reserved.

using System.Text.Json;

namespace Spatial.Helpers;

/// <summary>
/// Tests for <see cref="Json"/>.
/// </summary>
public class JsonHelperTests
{
    private enum Color
    {
        Red,
        Green,
        Blue
    }

    private class Address
    {
        public string Street { get; set; } = string.Empty;

        public string City { get; set; } = string.Empty;
    }

    private class Person
    {
        public string Name { get; set; } = string.Empty;

        public int Age { get; set; }

        public Color FavoriteColor { get; set; }

        public Address Address { get; set; } = new();

        public List<string> Tags { get; set; } = [];

        public Dictionary<string, int> Scores { get; set; } = [];

        public double? Rating { get; set; }
    }

    /// <summary>
    /// Generate a JSON schema for a simple string type.
    /// </summary>
    [Fact]
    [Trait("Category", "Unit")]
    public void TestSchemaForString()
    {
        using var document = JsonDocument.Parse(Json.Schema<string>());

        Assert.Equal("string", document.RootElement.GetProperty("type").GetString());
    }

    /// <summary>
    /// Generate a JSON schema for an enum type.
    /// </summary>
    [Fact]
    [Trait("Category", "Unit")]
    public void TestSchemaForEnum()
    {
        using var document = JsonDocument.Parse(Json.Schema<Color>());
        var root = document.RootElement;

        Assert.Equal("string", root.GetProperty("type").GetString());
        Assert.Equal(["Red", "Green", "Blue"], root.GetProperty("enum").EnumerateArray().Select(e => e.GetString()));
    }

    /// <summary>
    /// Generate a JSON schema for an unsigned integer type.
    /// </summary>
    [Fact]
    [Trait("Category", "Unit")]
    public void TestSchemaForUnsignedInteger()
    {
        using var document = JsonDocument.Parse(Json.Schema<uint>());
        var root = document.RootElement;

        Assert.Equal("integer", root.GetProperty("type").GetString());
        Assert.Equal(0, root.GetProperty("minimum").GetInt32());
    }

    /// <summary>
    /// Generate a JSON schema for a signed integer type.
    /// </summary>
    [Fact]
    [Trait("Category", "Unit")]
    public void TestSchemaForSignedInteger()
    {
        using var document = JsonDocument.Parse(Json.Schema<int>());
        var root = document.RootElement;

        Assert.Equal("integer", root.GetProperty("type").GetString());
        Assert.False(root.TryGetProperty("minimum", out _));
    }

    /// <summary>
    /// Generate a JSON schema for a nullable type, unwrapping to the underlying type.
    /// </summary>
    [Fact]
    [Trait("Category", "Unit")]
    public void TestSchemaForNullable()
    {
        using var document = JsonDocument.Parse(Json.Schema<double?>());

        Assert.Equal("number", document.RootElement.GetProperty("type").GetString());
    }

    /// <summary>
    /// Generate a JSON schema for an array type.
    /// </summary>
    [Fact]
    [Trait("Category", "Unit")]
    public void TestSchemaForArray()
    {
        using var document = JsonDocument.Parse(Json.Schema<int[]>());
        var root = document.RootElement;

        Assert.Equal("array", root.GetProperty("type").GetString());
        Assert.Equal("integer", root.GetProperty("items").GetProperty("type").GetString());
    }

    /// <summary>
    /// Generate a JSON schema for a string-keyed dictionary type.
    /// </summary>
    [Fact]
    [Trait("Category", "Unit")]
    public void TestSchemaForDictionary()
    {
        using var document = JsonDocument.Parse(Json.Schema<Dictionary<string, int>>());
        var root = document.RootElement;

        Assert.Equal("object", root.GetProperty("type").GetString());
        Assert.Equal("integer", root.GetProperty("additionalProperties").GetProperty("type").GetString());
    }

    /// <summary>
    /// Throw when a dictionary's key type is not a string.
    /// </summary>
    [Fact]
    [Trait("Category", "Unit")]
    public void TestSchemaForNonStringKeyedDictionaryThrows()
    {
        Assert.Throws<NotSupportedException>(() => Json.Schema<Dictionary<int, string>>());
    }

    /// <summary>
    /// Generate a JSON schema for a complex object type, including nested objects, collections, and required properties.
    /// </summary>
    [Fact]
    [Trait("Category", "Unit")]
    public void TestSchemaForObject()
    {
        using var document = JsonDocument.Parse(Json.Schema<Person>());
        var root = document.RootElement;

        Assert.Equal("object", root.GetProperty("type").GetString());
        Assert.False(root.GetProperty("additionalProperties").GetBoolean());

        var properties = root.GetProperty("properties");

        Assert.Equal("string", properties.GetProperty("Name").GetProperty("type").GetString());
        Assert.Equal("integer", properties.GetProperty("Age").GetProperty("type").GetString());
        Assert.Equal("string", properties.GetProperty("FavoriteColor").GetProperty("type").GetString());
        Assert.Equal("object", properties.GetProperty("Address").GetProperty("type").GetString());
        Assert.Equal("array", properties.GetProperty("Tags").GetProperty("type").GetString());
        Assert.Equal("object", properties.GetProperty("Scores").GetProperty("type").GetString());

        var required = root.GetProperty("required").EnumerateArray().Select(e => e.GetString());

        Assert.Equal(["Name", "Age", "FavoriteColor", "Address", "Tags", "Scores", "Rating"], required);
    }
}