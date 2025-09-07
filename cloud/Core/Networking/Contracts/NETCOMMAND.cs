// Copyright © Spatial Corporation. All rights reserved.

namespace Spatial.Networking.Contracts;

/// <summary>
/// A command issued over the network.
/// </summary>
public enum NETCOMMAND
{
    /// <summary>
    /// Issue a seed to a <see cref="Connection"/>.
    /// </summary>
    NC_MISC_SEED_CMD = 0x0807
}