// Copyright © Spatial Corporation. All rights reserved.

using Spatial.Cloud.Contracts.Intelligence.Nodes;
using Spatial.Networking;

namespace Spatial.Cloud.Controllers.Intelligence.Nodes;

/// <summary>
/// A <see cref="Controller"/> for <see cref="Input"/>.
/// </summary>
public class InputController : Controller
{
    /// <summary>
    /// Process raw <see cref="Input"/> data.
    /// </summary>
    /// <param name="input">Raw <see cref="Input"/> data sent to the server.</param>
    [Operation(0x0000)]
    public void Process(Input input)
    {
        Server.Current.Extractor.Push(input.Actuator, input.Data);
    }
}