// Copyright © Spatial. All rights reserved.

using Geneva.Components;

namespace Geneva;

/// <summary>
/// Constant values used in Geneva.
/// </summary>
public static class Constants
{
    /// <summary>
    /// The resting potential of a <see cref="Neuron"/> in millivolts (mV).
    /// </summary>
    public const double pR = -70.0D;

    /// <summary>
    /// The threshold potential of a <see cref="Neuron"/> in millivolts (mV).
    /// </summary>
    public const double pT = -55.0D;

    /// <summary>
    /// The action potential of a <see cref="Neuron"/> in millivolts (mV).
    /// </summary>
    public const double pA = 40.0D;

    /// <summary>
    /// The potential of a <see cref="Neuron"/> in its refractory period in millivolts (mV).
    /// </summary>
    public const double pRf = -90.0D;

    /// <summary>
    /// The decay rate of a <see cref="Neuron"/>.
    /// </summary>
    public const double λ = 50.0D;

    /// <summary>
    /// The depolarization rate of a <see cref="Neuron"/>.
    /// </summary>
    public const double rD = 50.0D;

    /// <summary>
    /// The refractory rate of a <see cref="Neuron"/>.
    /// </summary>
    public const double rR = 40.0D;

    /// <summary>
    /// The hyperpolarization rate of a <see cref="Neuron"/>.
    /// </summary>
    public const double rH = 30.0D;

    /// <summary>
    /// The temporal window for synaptic plasticity.
    /// </summary>
    public const double τ = 50.0D;

    /// <summary>
    /// The learning rate of a synaptic connection.
    /// </summary>
    public const double A = 0.1D;

    /// <summary>
    /// The decay rate of a synaptic connection.
    /// </summary>
    public const double Ø = 0.001D;
}