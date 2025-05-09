// Copyright © Spatial. All rights reserved.

using System;

namespace Ignite.Components;

/// <summary>
/// An adjustable value bound by a range.
/// </summary>
/// <param name="Minimum">The parameter's minimum value.</param>
/// <param name="Maximum">The parameter's maximum value.</param>
/// <param name="Current">The parameter's current value.</param>
public record struct Parameter(float Minimum = 0.0F, float Maximum = 0.0F, float Current = 0.0F) {
    /// <summary>
    /// Create a new <see cref="Parameter"/>.
    /// </summary>
    /// <param name="maximum">The parameter's maximum value.</param>
    public Parameter(float maximum) : this(0, maximum, maximum) { }

    /// <summary>
    /// Create a new <see cref="Parameter"/>.
    /// </summary>
    /// <param name="minimum">The parameter's minimum value.</param>
    /// <param name="maximum">The parameter's maximum value.</param>
    public Parameter(float minimum, float maximum) : this(minimum, maximum, maximum) { }

    /// <summary>
    /// The parameter's percentage value.
    /// </summary>
    public readonly float Percentage => (Current - Minimum) / (Maximum - Minimum) * 100F;

    /// <summary>
    /// Add a value to the <see cref="Parameter"/>.
    /// </summary>
    /// <param name="parameter">The <see cref="Parameter"/>.</param>
    /// <param name="value">The value to add to the <see cref="Parameter"/>.</param>
    /// <returns>A <see cref="Parameter"/>.</returns>
    public static Parameter operator +(in Parameter parameter, in float value) {
        return new Parameter(
            Minimum: parameter.Minimum,
            Maximum: parameter.Maximum,
            Current: Math.Clamp(parameter.Current + value, parameter.Minimum, parameter.Maximum));
    }

    /// <summary>
    /// Subtract a value from the <see cref="Parameter"/>.
    /// </summary>
    /// <param name="parameter">The <see cref="Parameter"/>.</param>
    /// <param name="value">The value to subtract from the <see cref="Parameter"/>.</param>
    /// <returns>A <see cref="Parameter"/>.</returns>
    public static Parameter operator -(in Parameter parameter, in float value) {
        return new Parameter(
            Minimum: parameter.Minimum,
            Maximum: parameter.Maximum,
            Current: Math.Clamp(parameter.Current - value, parameter.Minimum, parameter.Maximum));
    }

    /// <summary>
    /// Multiply the <see cref="Parameter"/> by a value.
    /// </summary>
    /// <param name="parameter">The <see cref="Parameter"/>.</param>
    /// <param name="value">The value to multiply the <see cref="Parameter"/> by.</param>
    /// <returns>A <see cref="Parameter"/>.</returns>
    public static Parameter operator *(in Parameter parameter, in float value) {
        return new Parameter(
            Minimum: parameter.Minimum,
            Maximum: parameter.Maximum,
            Current: Math.Clamp(parameter.Current * value, parameter.Minimum, parameter.Maximum));
    }

    /// <summary>
    /// Divide the <see cref="Parameter"/> by a value.
    /// </summary>
    /// <param name="parameter">The <see cref="Parameter"/>.</param>
    /// <param name="value">The value to divide the <see cref="Parameter"/> by.</param>
    /// <returns>A <see cref="Parameter"/>.</returns>
    public static Parameter operator /(in Parameter parameter, in float value) {
        return new Parameter(
            Minimum: parameter.Minimum,
            Maximum: parameter.Maximum,
            Current: Math.Clamp(parameter.Current / value, parameter.Minimum, parameter.Maximum));
    }

    /// <summary>
    /// Add a value to the <see cref="Parameter"/>.
    /// </summary>
    /// <param name="parameter">The <see cref="Parameter"/>.</param>
    /// <param name="value">The value to add to the <see cref="Parameter"/>.</param>
    /// <returns>A <see cref="Parameter"/>.</returns>
    public static Parameter operator +(in Parameter parameter, in Parameter value) {
        return new Parameter(
            Minimum: parameter.Minimum,
            Maximum: parameter.Maximum,
            Current: Math.Clamp(parameter.Current + value.Current, parameter.Minimum, parameter.Maximum));
    }

    /// <summary>
    /// Subtract a value from the <see cref="Parameter"/>.
    /// </summary>
    /// <param name="parameter">The <see cref="Parameter"/>.</param>
    /// <param name="value">The value to subtract from the <see cref="Parameter"/>.</param>
    /// <returns>A <see cref="Parameter"/>.</returns>
    public static Parameter operator -(in Parameter parameter, in Parameter value) {
        return new Parameter(
            Minimum: parameter.Minimum,
            Maximum: parameter.Maximum,
            Current: Math.Clamp(parameter.Current - value.Current, parameter.Minimum, parameter.Maximum));
    }

    /// <summary>
    /// Multiply the <see cref="Parameter"/> by a value.
    /// </summary>
    /// <param name="parameter">The <see cref="Parameter"/>.</param>
    /// <param name="value">The value to multiply the <see cref="Parameter"/> by.</param>
    /// <returns>A <see cref="Parameter"/>.</returns>
    public static Parameter operator *(in Parameter parameter, in Parameter value) {
        return new Parameter(
            Minimum: parameter.Minimum,
            Maximum: parameter.Maximum,
            Current: Math.Clamp(parameter.Current * value.Current, parameter.Minimum, parameter.Maximum));
    }

    /// <summary>
    /// Divide the <see cref="Parameter"/> by a value.
    /// </summary>
    /// <param name="parameter">The <see cref="Parameter"/>.</param>
    /// <param name="value">The value to divide the <see cref="Parameter"/> by.</param>
    /// <returns>A <see cref="Parameter"/>.</returns>
    public static Parameter operator /(in Parameter parameter, in Parameter value) {
        return new Parameter(
            Minimum: parameter.Minimum,
            Maximum: parameter.Maximum,
            Current: Math.Clamp(parameter.Current / value.Current, parameter.Minimum, parameter.Maximum));
    }

    /// <summary>
    /// Set the <see cref="Parameter"/> to a value.
    /// </summary>
    /// <param name="value">The value to set the <see cref="Parameter"/> to.</param>
    /// <returns>The <see cref="Parameter"/>.</returns>
    public void Set(in Parameter value) {
        Set(value.Current);
    }

    /// <summary>
    /// Set the <see cref="Parameter"/> to a value.
    /// </summary>
    /// <param name="value">The value to set the <see cref="Parameter"/> to.</param>
    /// <returns>The <see cref="Parameter"/>.</returns>
    public void Set(in float value) {
        Current = Math.Clamp(value, Minimum, Maximum);
    }
}