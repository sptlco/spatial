
using System.Text.Json;
using MongoDB.Bson;

namespace Spatial.Extensions;

/// <summary>
/// Extension methods for <see cref="object"/>.
/// </summary>
public static class ObjectExtensions
{
    /// <summary>
    /// Convert an <see cref="object"/> to Bson.
    /// </summary>
    /// <param name="input">An object.</param>
    /// <returns>The converted object.</returns>
    public static BsonDocument AsBson(this object? input)
    {
        if (input is null)
        {
            return [];
        }

        if (input is BsonDocument bson)
        {
            return bson;
        }

        if (input is JsonElement json)
        {
            return BsonDocument.Parse(json.GetRawText());
        }

        return input.ToBsonDocument();
    }
}