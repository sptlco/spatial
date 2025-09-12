// Copyright © Spatial Corporation. All rights reserved.

namespace Spatial.Networking;

/// <summary>
/// A command issued over the private network.
/// </summary>
[AttributeUsage(AttributeTargets.Method)]
public class OperationAttribute : Attribute
{
    /// <summary>
    /// Create a new <see cref="OperationAttribute"/>.
    /// </summary>
    /// <param name="code">The operation's code number.</param>
    public OperationAttribute(ushort code)
    {
        Code = code;
    }

    /// <summary>
    /// The operation's code number.
    /// </summary>
    public ushort Code { get; }
}