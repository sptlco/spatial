// Copyright © Spatial. All rights reserved.

using Ignite.Contracts;
using Ignite.Models;
using Spatial.Simulation;

namespace Ignite.Components;

/// <summary>
/// An object in the <see cref="World"/>.
/// </summary>
/// <param name="Mode">The object's current <see cref="ObjectMode"/>.</param>
public record struct Object(ObjectMode Mode = ObjectMode.Default) : IComponent;