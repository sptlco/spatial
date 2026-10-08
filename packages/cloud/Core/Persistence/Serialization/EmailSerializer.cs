// Copyright © Spatial Corporation. All rights reserved.

using MongoDB.Bson.Serialization;
using Spatial.Identity;

namespace Spatial.Persistence.Serialization;

/// <summary>
/// Serializes <see cref="Email"/> as a bare BSON string.
/// </summary>
/// <remarks>
/// <para>
/// Writes and reads the same document shape as the <c>string</c> field it
/// replaced — <c>{ "email": "user@example.com" }</c>, not
/// <c>{ "email": { "value": "..." } }</c>. This is load-bearing: existing
/// account documents were written before <see cref="Email"/> existed, and
/// any change in shape would silently fail to deserialize them.
/// </para>
/// <para>
/// Deserialization routes through <see cref="Email.Parse"/>, so a legacy
/// document with mixed casing or stray whitespace normalizes on read and
/// comes back corrected on its next write.
/// </para>
/// </remarks>
/// <example>
/// <code>
/// // Once, at startup.
/// BsonSerializer.RegisterSerializer(new EmailSerializer());
/// </code>
/// </example>
public class EmailSerializer : IBsonSerializer<Email>
{
    /// <summary>
    /// The type this serializer handles.
    /// </summary>
    public Type ValueType => typeof(Email);

    /// <summary>
    /// Read an <see cref="Email"/> from BSON.
    /// </summary>
    /// <param name="context">The deserialization context.</param>
    /// <param name="args">The deserialization arguments.</param>
    /// <returns>The normalized address.</returns>
    /// <exception cref="ArgumentException">
    /// The stored value is not a valid email address.
    /// </exception>
    public Email Deserialize(BsonDeserializationContext context, BsonDeserializationArgs args) => Email.Parse(context.Reader.ReadString());

    /// <summary>
    /// Write an <see cref="Email"/> to BSON.
    /// </summary>
    /// <param name="context">The serialization context.</param>
    /// <param name="args">The serialization arguments.</param>
    /// <param name="value">The address to write.</param>
    public void Serialize(BsonSerializationContext context, BsonSerializationArgs args, Email value) => context.Writer.WriteString(value.Value);

    /// <summary>
    /// Write an <see cref="Email"/> to BSON through the non-generic interface.
    /// </summary>
    /// <param name="context">The serialization context.</param>
    /// <param name="args">The serialization arguments.</param>
    /// <param name="value">The address to write.</param>
    public void Serialize(BsonSerializationContext context, BsonSerializationArgs args, object value) => Serialize(context, args, (Email) value);

    /// <summary>
    /// Read an <see cref="Email"/> from BSON through the non-generic interface.
    /// </summary>
    /// <param name="context">The deserialization context.</param>
    /// <param name="args">The deserialization arguments.</param>
    /// <returns>The normalized address.</returns>
    object IBsonSerializer.Deserialize(BsonDeserializationContext context, BsonDeserializationArgs args) => Deserialize(context, args);
}