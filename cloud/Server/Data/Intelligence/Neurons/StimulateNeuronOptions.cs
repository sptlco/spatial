// Copyright © Spatial Corporation. All rights reserved.

namespace Spatial.Cloud.Data.Intelligence.Neurons;

/// <summary>
/// Configurable options for neuron stimulation.
/// </summary>
public class StimulateNeuronOptions
{
    /// <summary>
    /// The stimulus for the <see cref="Neuron"/>.
    /// </summary>
    public double Charge { get; set; }
}