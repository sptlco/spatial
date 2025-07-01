// Copyright © Spatial Corporation. All rights reserved.

namespace Spatial.Simulation;

/// <summary>
/// A pair of objects.
/// </summary>
public readonly record struct Pair
{
    private readonly uint _first;
    private readonly uint _second;

    /// <summary>
    /// Create a new <see cref="Pair"/>.
    /// </summary>
    /// <param name="first">The first object in the <see cref="Pair"/>.</param>
    /// <param name="second">The second object in the <see cref="Pair"/>.</param>
    public Pair(uint first, uint second)
    {
        if (first > second)
        {
            (first, second) = (second, first);
        }

        _first = first;
        _second = second;
    }

    /// <summary>
    /// The first object in the <see cref="Pair"/>.
    /// </summary>
    public readonly uint First => _first;

    /// <summary>
    /// The second object in the <see cref="Pair"/>.
    /// </summary>
    public readonly uint Second => _second;

    /// <summary>
    /// Get whether or not the <see cref="Pair"/> equals another <see cref="Pair"/>.
    /// </summary>
    /// <param name="other">Another <see cref="Pair"/>.</param>
    /// <returns>Whether or not the two pairs are equal.</returns>
    public readonly bool Equals(Pair other) => First == other.First && Second == other.Second || First == other.Second && Second == other.First;

    /// <summary>
    /// Get the hash code of the <see cref="Pair"/>.
    /// </summary>
    /// <returns>The pair's hash code.</returns>
    public override readonly int GetHashCode() => HashCode.Combine(First, Second);
}