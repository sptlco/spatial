// Copyright © Spatial Corporation. All rights reserved.

using Spatial.Cloud.Models.Neurons;
using Spatial.Simulation;

namespace Spatial.Cloud.Components;

/// <summary>
/// ...
/// </summary>
public record struct Neuron(NeuronType Type, int Group, int Channel, double Value) : IComponent;