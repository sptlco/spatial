// Copyright © Spatial. All rights reserved.

using System.Security.Cryptography;

namespace Spatial.Mathematics;

/// <summary>
/// A cryptographically strong value generator.
/// </summary>
public static class Strong
{
    /// <summary>
    /// Get a random <see cref="bool"/>.
    /// </summary>
    /// <returns>A <see cref="bool"/>.</returns>
    public static bool Boolean()
    {
        return Boolean(0.5F);
    }

    /// <summary>
    /// Get a random <see cref="bool"/>.
    /// </summary>
    /// <param name="chance">The chance of the <see cref="bool"/> being true.</param>
    /// <returns>A <see cref="bool"/>.</returns>
    public static bool Boolean(float chance)
    {
        return Float(100) < 100 * chance;
    }

    /// <summary>
    /// Generate a strong <see cref="float"/>.
    /// </summary>
    /// <returns>A <see cref="float"/>.</returns>
    public static float Float()
    {
        return Float(float.MaxValue);
    }

    /// <summary>
    /// Generate a strong <see cref="float"/>.
    /// </summary>
    /// <param name="max">The exclusive maximum value that can be generated.</param>
    /// <returns>A <see cref="float"/>.</returns>
    public static float Float(float max)
    {
        return Float(0, max);
    }

    /// <summary>
    /// Generate a strong <see cref="float"/>.
    /// </summary>
    /// <param name="min">The inclusive minimum value that can be generated.</param>
    /// <param name="max">The exclusive maximum value that can be generated.</param>
    /// <returns>A <see cref="float"/>.</returns>
    public static float Float(float min, float max)
    {
        return (float) Double(min, max);
    }

    /// <summary>
    /// Generate a strong <see cref="double"/>.
    /// </summary>
    /// <returns>A <see cref="double"/>.</returns>
    public static double Double()
    {
        return Double(double.MaxValue);
    }

    /// <summary>
    /// Generate a strong <see cref="double"/>.
    /// </summary>
    /// <param name="max">The exclusive maximum value that can be generated.</param>
    /// <returns>A <see cref="double"/>.</returns>
    public static double Double(double max)
    {
        return Double(0, max);
    }

    /// <summary>
    /// Generate a strong <see cref="double"/>.
    /// </summary>
    /// <param name="min">The inclusive minimum value that can be generated.</param>
    /// <param name="max">The exclusive maximum value that can be generated.</param>
    /// <returns>A <see cref="double"/>.</returns>
    public static double Double(double min, double max)
    {
        return min + Int32() / (double) int.MaxValue * (max - min);
    }

    /// <summary>
    /// Generate a strong <see cref="byte"/>.
    /// </summary>
    /// <returns>A <see cref="byte"/>.</returns>
    public static byte Byte()
    {
        return Byte(byte.MaxValue);
    }

    /// <summary>
    /// Generate a strong <see cref="byte"/>.
    /// </summary>
    /// <param name="max">The exclusive maximum value that can be generated.</param>
    /// <returns>A <see cref="byte"/>.</returns>
    public static byte Byte(byte max)
    {
        return Byte(0, max);
    }

    /// <summary>
    /// Generate a strong <see cref="byte"/>.
    /// </summary>
    /// <param name="min">The inclusive minimum value that can be generated.</param>
    /// <param name="max">The exclusive maximum value that can be generated.</param>
    /// <returns>A <see cref="byte"/>.</returns>
    public static byte Byte(byte min, byte max)
    {
        return (byte) Int32(min, max);
    }

    /// <summary>
    /// Generate a strong <see cref="ushort"/>.
    /// </summary>
    /// <returns>A <see cref="ushort"/>.</returns>
    public static ushort UInt16()
    {
        return UInt16(ushort.MaxValue);
    }

    /// <summary>
    /// Generate a strong <see cref="ushort"/>.
    /// </summary>
    /// <param name="max">The exclusive maximum value that can be generated.</param>
    /// <returns>A <see cref="ushort"/>.</returns>
    public static ushort UInt16(ushort max)
    {
        return UInt16(0, max);
    }

    /// <summary>
    /// Generate a strong <see cref="ushort"/>.
    /// </summary>
    /// <param name="min">The inclusive minimum value that can be generated.</param>
    /// <param name="max">The exclusive maximum value that can be generated.</param>
    /// <returns>An <see cref="ushort"/>.</returns>
    public static ushort UInt16(ushort min, ushort max)
    {
        return (ushort) Int32(min, max);
    }

    /// <summary>
    /// Generate a strong <see cref="int"/>.
    /// </summary>
    /// <returns>An <see cref="int"/>.</returns>
    public static int Int32()
    {
        return Int32(int.MaxValue);
    }

    /// <summary>
    /// Generate a strong <see cref="int"/>.
    /// </summary>
    /// <param name="max">The exclusive maximum value that can be generated.</param>
    /// <returns>An <see cref="int"/>.</returns>
    public static int Int32(int max)
    {
        return Int32(0, max);
    }

    /// <summary>
    /// Generate a strong <see cref="int"/>.
    /// </summary>
    /// <param name="min">The inclusive minimum value that can be generated.</param>
    /// <param name="max">The exclusive maximum value that can be generated.</param>
    /// <returns>An <see cref="int"/>.</returns>
    public static int Int32(int min, int max)
    {
        return RandomNumberGenerator.GetInt32(min, max);
    }

    /// <summary>
    /// Generate a strong <see cref="long"/>.
    /// </summary>
    /// <returns>A <see cref="long"/>.</returns>
    public static long Int64()
    {
        return Int64(long.MaxValue);
    }

    /// <summary>
    /// Generate a strong <see cref="long"/>.
    /// </summary>
    /// <param name="max">The exclusive maximum value that can be generated.</param>
    /// <returns>A <see cref="long"/>.</returns>
    public static long Int64(long max)
    {
        return Int64(0, max);
    }

    /// <summary>
    /// Generate a strong <see cref="long"/>.
    /// </summary>
    /// <param name="min">The inclusive minimum value that can be generated.</param>
    /// <param name="max">The exclusive maximum value that can be generated.</param>
    /// <returns>A <see cref="long"/>.</returns>
    public static long Int64(long min, long max)
    {
        return (long) Double(min, max);
    }

    /// <summary>
    /// Generate a strong array of bytes.
    /// </summary>
    /// <param name="count">The number of bytes to generate.</param>
    /// <returns>An array of bytes.</returns>
    public static byte[] Bytes(int count)
    {
        return RandomNumberGenerator.GetBytes(count);
    }
}