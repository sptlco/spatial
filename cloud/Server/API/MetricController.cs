// Copyright © Spatial Corporation. All rights reserved.

using Spatial.Cloud.Data.Scopes;
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
    /// <param name="resolution">Optional search resolution.</param>
    /// <returns>A list of matching <see cref="Metric"/> records.</returns>
    [GET]
    [Path("{name}")]
    [Authorize(Scope.Metrics.Read)]
    public Task<List<Metric>> ReadAsync(
        string name,
        [Query] DateTime? from = null,
        [Query] DateTime? to = null,
        [Query] int? limit = null,
        [Query] string? resolution = null)
    {
        return Metric.ReadAsync(name, from, to, limit, resolution);
    }
}