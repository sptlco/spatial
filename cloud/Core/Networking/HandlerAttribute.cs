// Copyright © Spatial Corporation. All rights reserved.

namespace Spatial.Networking;

/// <summary>
/// Specifies a method as handling a specific 
/// </summary>
[AttributeUsage(AttributeTargets.Method)]
public class HandlerAttribute : Attribute
{
    /// <summary>
    /// Create a new <see cref="HandlerAttribute"/>.
    /// </summary>
    /// <param name="command">A network command.</param>
    public HandlerAttribute(ushort command)
    {
        Command = command;
    }

    /// <summary>
    /// The command handled by the method.
    /// </summary>
    public ushort Command { get; }
}