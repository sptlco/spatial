// Copyright © Spatial Corporation. All rights reserved.

namespace Spatial.Cloud.Data;

/// <summary>
/// A semantically formatted version number.
/// </summary>
public class Version
{
    /// <summary>
    /// Create a new <see cref="Version"/>.
    /// </summary>
    public Version() { }

    /// <summary>
    /// Create a new <see cref="Version"/>.
    /// </summary>
    /// <param name="version">A major and minor <see cref="Version"/> number.</param>
    public Version(double version)
    {
        ConvertFromString(version.ToString());
    }

    /// <summary>
    /// Create a new <see cref="Version"/>.
    /// </summary>
    /// <param name="version">A semantically formatted <see cref="Version"/> string.</param>
    public Version(string version)
    {
        ConvertFromString(version);
    }

    /// <summary>
    /// The version's major number.
    /// </summary>
    public int Major { get; set; }

    /// <summary>
    /// The version's minor number.
    /// </summary>
    public int Minor { get; set; }

    /// <summary>
    /// The version's patch number.
    /// </summary>
    public int Patch { get; set; }

    /// <summary>
    /// Get a string representation of the <see cref="Version"/>.
    /// </summary>
    /// <returns>A string representation of the <see cref="Version"/>.</returns>
    public override string ToString()
    {
        return $"{Major}.{Minor}.{Patch}";
    }

    private void ConvertFromString(string version)
    {
        var parts = version.Split(".");

        Major = int.Parse(parts[0]);

        if (parts.Length > 1)
        {
            Minor = int.Parse(parts[1]);
        }

        if (parts.Length > 2)
        {
            Patch = int.Parse(parts[2]);
        }
    }
}