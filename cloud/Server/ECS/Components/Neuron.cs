// Copyright © Spatial Corporation. All rights reserved.

using Spatial.Cloud.Data.Neurons;
using Spatial.Simulation;

namespace Spatial.Cloud.ECS.Components;

/// <summary>
/// ...
/// </summary>
public record struct Neuron(NeuronType Type, int Group, int Channel, double Value) : IComponent;