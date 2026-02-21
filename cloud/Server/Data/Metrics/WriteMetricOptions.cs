// Copyright © Spatial Corporation. All rights reserved.

namespace Spatial.Cloud.Data.Metrics;

/// <summary>
/// Configurable options for a new metric.
/// </summary>
public class WriteMetricOptions
{
    /// <summary>
    /// Contextual data about the metric.
    /// </summary>
    public Dictionary<string, string> Metadata { get; set; }

    /// <summary>
    /// The metric's data point(s).
    /// </summary>
    public Dictionary<string, decimal> Value { get; set; }
}