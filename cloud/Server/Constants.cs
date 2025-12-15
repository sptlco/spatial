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
    public const string Alphanumerics = "ABCDEFGHIJKLMNOPQRSTUVWXYZ0123456789";

    /// <summary>
    /// The length of a key.
    /// </summary>
    public const int KeyLength = 4;
}

/// <summary>
/// Constant permissions used for authorization.
/// </summary>
internal static class Permissions
{
    public class Users
    {
        public const string Read = "users.read";
    }
}