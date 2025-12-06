// Copyright © Spatial Corporation. All rights reserved.

namespace Spatial.Cloud;

/// <summary>
/// Constant values used by the cloud server.
/// </summary>
internal static class Constants
{
    /// <summary>
    /// The name of the system.
    /// </summary>
    public const string Spatial = "spatial";

    /// <summary>
    /// Numerical digits.
    /// </summary>
    public const string Digits = "0123456789";

    /// <summary>
    /// The length of a key.
    /// </summary>
    public const int KeyLength = 4;
}

/// <summary>
/// Constant property names.
/// </summary>
internal static class Properties
{
    public const string Subject = "Subject";

    public const string Code = "Code";

    public const string Expires = "Expires";
}