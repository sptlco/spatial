// Copyright © Spatial Corporation. All rights reserved.

using System.Text.Json;
using System.Text.Json.Serialization;
using Spatial.Identity;

namespace Spatial.Serialization;

/// <summary>
/// Serializes <see cref="Email"/> as a bare JSON string.
/// </summary>
/// <remarks>
/// <para>
/// Without this, <see cref="Email"/> — a <c>readonly record struct</c> with a
/// public <see cref="Email.Value"/> property — serializes as
/// <c>{ "value": "user@example.com" }</c>, because that is what
/// <see cref="System.Text.Json"/> does with any type it does not have a
/// converter for. Clients expect a string and break on the object.
/// </para>
/// <para>
/// Reads route through <see cref="Email.Parse"/>, so a client that sends a
/// raw, unnormalized address has it normalized at the boundary — the same
/// guarantee the write path already has.
/// </para>
/// </remarks>
public class EmailJsonConverter : JsonConverter<Email>
{
    /// <summary>
    /// Read an <see cref="Email"/> from JSON.
    /// </summary>
    /// <param name="reader">The reader positioned at the value.</param>
    /// <param name="typeToConvert">The type to convert.</param>
    /// <param name="options">The serializer options.</param>
    /// <returns>The normalized address.</returns>
    /// <exception cref="JsonException">
    /// The token is not a string, or is not a valid email address.
    /// </exception>
    public override Email Read(ref Utf8JsonReader reader, Type typeToConvert, JsonSerializerOptions options)
    {
        if (reader.TokenType != JsonTokenType.String)
        {
            throw new JsonException($"Expected a string for {nameof(Email)}, got {reader.TokenType}.");
        }

        try
        {
            return Email.Parse(reader.GetString()!);
        }
        catch (ArgumentException e)
        {
            throw new JsonException(e.Message, e);
        }
    }

    /// <summary>
    /// Write an <see cref="Email"/> to JSON.
    /// </summary>
    /// <param name="writer">The writer to write to.</param>
    /// <param name="value">The address to write.</param>
    /// <param name="options">The serializer options.</param>
    public override void Write(Utf8JsonWriter writer, Email value, JsonSerializerOptions options) => writer.WriteStringValue(value.Value);
}