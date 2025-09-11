// Copyright © Spatial Corporation. All rights reserved.

using System.Reflection;
using System.Text.Json;

namespace Spatial.Cloud.Helpers;

/// <summary>
/// Helper methods for JSON.
/// </summary>
public class Json
{
    /// <summary>
    /// Generate a JSON schema for type <typeparamref name="T"/>.
    /// </summary>
    /// <typeparam name="T">The type to generate a schema for.</typeparam>
    /// <returns>A JSON schema for type <typeparamref name="T"/>.</returns>
    public static string Schema<T>() => JsonSerializer.Serialize(BuildSchema(typeof(T)), new JsonSerializerOptions { WriteIndented = true });

    private static object BuildSchema(Type type)
    {
        if (Nullable.GetUnderlyingType(type) is Type underlying)
        {
            type = underlying;
        }

        if (type.IsEnum)
        {
            return new
            {
                type = "string",
                @enum = Enum.GetNames(type)
            };
        }

        if (type == typeof(string))
        {
            return new { type = "string" };
        }

        if (type == typeof(bool))
        {
            return new { type = "boolean" };
        }

        if (type == typeof(byte) || type == typeof(ushort) || type == typeof(uint) || type == typeof(ulong))
        {
            return new { type = "integer", minimum = 0 };
        }

        if (type == typeof(sbyte) || type == typeof(short) || type == typeof(int) || type == typeof(long))
        {
            return new { type = "integer" };
        }

        if (type == typeof(float) || type == typeof(double) || type == typeof(decimal))
        {
            return new { type = "number" };
        }

        if (type.IsGenericType && type.GetGenericTypeDefinition() == typeof(Dictionary<,>))
        {
            var key = type.GetGenericArguments()[0];
            var value = type.GetGenericArguments()[1];

            if (key != typeof(string))
            {
                throw new NotSupportedException("Only Dictionary<string, T> is supported in schema.");
            }

            return new {
                type = "object",
                additionalProperties = BuildSchema(value)
            };
        }

        if (type.IsArray || type.GetInterfaces().Any(i => i.IsGenericType && i.GetGenericTypeDefinition() == typeof(IEnumerable<>)))
        {
            var element = type.IsArray ? type.GetElementType()! : type
                .GetInterfaces()
                .First(i => i.IsGenericType && i.GetGenericTypeDefinition() == typeof(IEnumerable<>))
                .GetGenericArguments()[0];

            return new {
                type = "array",
                items = BuildSchema(element)
            };
        }

        if (type.IsClass || type.IsValueType)
        {
            var props = type.GetProperties(BindingFlags.Public | BindingFlags.Instance);

            return new {
                type = "object",
                properties = props.ToDictionary(p => p.Name, p => BuildSchema(p.PropertyType)),
                required = props.Select(p => p.Name).ToArray(),
                additionalProperties = false
            };
        }

        return new { type = "string" };
    }

}
