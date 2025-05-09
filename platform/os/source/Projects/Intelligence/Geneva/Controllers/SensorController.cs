// Copyright © Spatial. All rights reserved.

using Geneva.Contracts;
using Geneva.Contracts.Sensors;
using Geneva.Models;
using Spatial.Networking;

namespace Geneva.Controllers;

/// <summary>
/// A <see cref="Controller"/> for sensor functions.
/// </summary>
public class SensorController : Controller
{
    /// <summary>
    /// Handle <see cref="NETCOMMAND.NC_SENSOR_STIMULATE_CHANNEL_CMD"/>.
    /// </summary>
    /// <param name="data">A <see cref="ProtocolBuffer"/>.</param>
    [NETHANDLER(NETCOMMAND.NC_SENSOR_STIMULATE_CHANNEL_CMD)]
    public void NC_SENSOR_STIMULATE_CHANNEL_CMD(PROTO_NC_SENSOR_STIMULATE_CHANNEL_CMD data)
    {
        Schematic.Stream(data.Channel, data.Intensity);
    }

    /// <summary>
    /// Handle <see cref="NETCOMMAND.NC_SENSOR_STIMULATE_BATCH_CMD"/>.
    /// </summary>
    /// <param name="data">A <see cref="ProtocolBuffer"/>.</param>
    [NETHANDLER(NETCOMMAND.NC_SENSOR_STIMULATE_BATCH_CMD)]
    public void NC_SENSOR_STIMULATE_BATCH_CMD(PROTO_NC_SENSOR_STIMULATE_BATCH_CMD data)
    {
        Schematic.Stream(data.Stimuli);
    }
}