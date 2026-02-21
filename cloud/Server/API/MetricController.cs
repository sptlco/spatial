// Copyright © Spatial Corporation. All rights reserved.

using Spatial.Cloud.Data.Metrics;
using Spatial.Cloud.Data.Scopes;
using Spatial.Extensions;
using Spatial.Identity.Authorization;
using Spatial.Persistence;

namespace Spatial.Cloud.API;

/// <summary>
/// A <see cref="Controller"/> for metric data.
/// </summary>
[Path("metrics")]
public class MetricController : Controller
{
    /// <summary>
    /// Reads data points for the specified metric.
    /// </summary>
    /// <param name="name">The metric's name.</param>
    /// <param name="from">Optional start timestamp (UTC).</param>
    /// <param name="to">Optional end timestamp (UTC).</param>
    /// <param name="limit">Optional maximum number of records to return.</param>
    /// <returns>A list of matching <see cref="Metric"/> records.</returns>
    [GET]
    [Path("{name}")]
    [Authorize(Scope.Metrics.Read)]
    public async Task<List<Metric>> ReadAsync(
        string name,
        [Query] DateTime? from = null,
        [Query] DateTime? to = null,
        [Query] int? limit = null)
    {
        var metrics = Resource<Metric>.List(metric => metric.Metadata[nameof(name)] == name);

        if (from.HasValue)
        {
            metrics = metrics.Filter(metric => metric.Timestamp >= from);
        }

        if (to.HasValue)
        {
            metrics = metrics.Filter(metric => metric.Timestamp <= to);
        }

        return limit.HasValue ? [..metrics.Take(limit.Value)] : metrics;
    }

    /// <summary>
    /// Create a new <see cref="Metric"/>.
    /// </summary>
    /// <param name="options">Configurable options for the <see cref="Metric"/>.</param>
    /// <returns>A <see cref="Metric"/>.</returns>
    [POST]
    [Authorize(Scope.Metrics.Write)]
    public async Task<Metric> WriteAsync([Body] WriteMetricOptions options)
    {
        var metric = new Metric {
            Metadata = options.Metadata,
            Value = options.Value
        };

        return metric.Store();
    }
}