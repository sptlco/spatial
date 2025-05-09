// Copyright © Spatial. All rights reserved.

using Spatial.Networking;
using System;

namespace Ignite.Contracts;

/// <summary>
/// Specifies that a method handles a network protocol.
/// </summary>
[AttributeUsage(AttributeTargets.Method)]
public class NETHANDLERAttribute : HandlerAttribute
{
    /// <summary>
    /// Create a new <see cref="NETHANDLERAttribute"/>.
    /// </summary>
    /// <param name="command">The <see cref="NETCOMMAND"/> that was issued.</param>
    public NETHANDLERAttribute(NETCOMMAND command) : base((ushort) command) {}
}