// Copyright © Spatial Corporation. All rights reserved.

namespace Spatial.Persistence;

/// <summary>
/// An expiration date.
/// </summary>
public enum Expiration : long
{
    /// <summary>
    /// The resource does not expire.
    /// </summary>
    None = 0,

    /// <summary>
    /// The <see cref="Resource"/> expires in one year.
    /// </summary>
    Year = (long) 365 * 24 * 60 * 60 * 1000
}