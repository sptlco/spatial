// Copyright © Spatial Corporation. All rights reserved.

using Microsoft.AspNetCore.Mvc;

namespace Spatial.Networking;

/// <summary>
/// Indicates that a <see cref="Controller"/> endpoint parameter is read from a JSON message body.
/// </summary>
public class BodyAttribute : FromBodyAttribute
{
}