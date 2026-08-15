// Copyright © Spatial Corporation. All rights reserved.

namespace Spatial.Persistence;

/// <summary>
/// A <see cref="Resource"/> that has an auto-incrementing integer identifier
/// in addition to its string <see cref="Resource.Id"/>.
/// </summary>
public interface ISequential
{
    /// <summary>
    /// The auto-incrementing sequence number, assigned on first insert.
    /// </summary>
    public uint Sequence { get; set; }
}