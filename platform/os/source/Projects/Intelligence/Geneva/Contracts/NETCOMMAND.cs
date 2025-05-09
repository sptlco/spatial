// Copyright © Spatial. All rights reserved.

namespace Geneva.Contracts;

/// <summary>
/// A command issued over the network.
/// </summary>
public enum NETCOMMAND
{
    /// <summary>
    /// Stimulate a single channel of the brain.
    /// </summary>
    NC_SENSOR_STIMULATE_CHANNEL_CMD = 0x00,

    /// <summary>
    /// Stimulate the brain.
    /// </summary>
    NC_SENSOR_STIMULATE_BATCH_CMD = 0x01,

    /// <summary>
    /// Broadcast motor outputs.
    /// </summary>
    NC_MOTOR_OUTPUT_CMD = 0x02
}