// Copyright © Spatial Corporation. All rights reserved.

using System.Text.RegularExpressions;

namespace Spatial.Extensions;

/// <summary>
/// Extension methods for <see cref="string"/>.
/// </summary>
public static class StringExtensions
{
    /// <summary>
    /// Get whether or not a string matches a wildcard value.
    /// </summary>
    /// <param name="value">The value to test.</param>
    /// <param name="wildcard">The wildcard value.</param>
    /// <returns>Whether or not the two strings match.</returns>
    public static bool Matches(this string value, string wildcard)
    {
        return Regex.IsMatch(value, "^" + Regex.Escape(wildcard).Replace("\\?", ".").Replace("\\*", ".*") + "$");
    }

    /// <summary>
    /// Normalize a path string.
    /// </summary>
    /// <param name="value">The string to normalize.</param>
    /// <returns>The normalized path.</returns>
    public static string ToNormalizedPath(this string value)
    {
        return value.Replace("/", "\\");
    }

    /// <summary>
    /// Convert a C-style string format to a positional string format.
    /// </summary>
    /// <param name="format">A C-style string format.</param>
    /// <returns>A positional string format.</returns>
    public static string ToPositionalFormat(this string format)
    {
        var tempMarker = Guid.NewGuid().ToString();
        var tempFormat = format.Replace("%%", tempMarker);

        var index = 0;
        var csharpFormat = Regex.Replace(tempFormat, "%[a-zA-Z]", m => $"{{{index++}}}");

        return csharpFormat.Replace(tempMarker, "%");
    }
}
