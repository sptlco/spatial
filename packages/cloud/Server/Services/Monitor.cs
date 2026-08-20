// Copyright © Spatial Corporation. All rights reserved.

using Spatial.Helpers;
using Spatial.Persistence;
using System.Diagnostics;
using System.Text.RegularExpressions;

namespace Spatial.Cloud.Services;

/// <summary>
/// A local performance monitor.
/// </summary>
public class Monitor : BackgroundService
{
    private readonly MonitorConfiguration _config;
    private readonly Cache _cache;

    private DateTime? _previousCpuStartTime;
    private TimeSpan? _previousTotalProcessorTime;

    private DateTime _notified
    {
        get => _cache.TryGet<DateTime>("monitor", "notified", out var value) ? value : DateTime.MinValue;
        set => _cache.Set("monitor", "notified", value);
    }

    /// <summary>
    /// Create a new <see cref="Monitor"/>.
    /// </summary>
    public Monitor()
    {
        _cache = new Cache(Application.Current.Configuration.Cache.Url);
        _config = Server.Current.Configuration.Monitor;
    }

    /// <summary>
    /// Continuously monitor the performance of the <see cref="Server"/>.
    /// </summary>
    /// <param name="token">A <see cref="CancellationToken"/> to stop service execution.</param>
    protected override async Task ExecuteAsync(CancellationToken token)
    {
        while (!token.IsCancellationRequested)
        {
            var next = DateTime.UtcNow.AddSeconds(5);

            try
            {
                var load = GetLoad();
                var memory = GetMemory();

                var metric = await Metric.WriteOneAsync("performance", new Dictionary<string, decimal> {
                    ["Load"] = (decimal) load,
                    ["WorkingSet64"] = memory.WorkingSet64,
                    ["PrivateMemorySize64"] = memory.PrivateMemorySize64,
                    ["VirtualMemorySize64"] = memory.VirtualMemorySize64
                });

                if (!string.IsNullOrEmpty(_config.Alerts))
                {
                    await ProcessMetricAsync(metric);
                    await TryNotifyAsync();
                }
            }
            catch (Exception e)
            {
                ERROR(e, "Failed to monitor performance due to an unexpected error.");
            }
            finally
            {
                await Task.Delay(TimeSpan.FromMilliseconds(Math.Max((next - DateTime.UtcNow).TotalMilliseconds, 10)), token);
            }
        }
    }
    
    private async Task ProcessMetricAsync(Metric metric)
    {
        foreach (var anomaly in GetAnomalies(metric))
        {
            WARN("Detected an anomaly ({Metric}): {Value} {Condition} {Threshold}", anomaly.Key.Metric, anomaly.Value.Value, anomaly.Key.Condition, anomaly.Key.Threshold);
        }
    }

    private async Task TryNotifyAsync()
    {
        if (DateTime.UtcNow - _notified < _config.NotificationInterval)
        {
            return;
        }

        var metrics = await Metric.ReadAsync("performance", _notified, DateTime.UtcNow);
        var anomalies = new Dictionary<(string Metric, string Condition, decimal Threshold), (DateTime Timestamp, decimal Value)>();

        metrics.ForEach(metric => GetAnomalies(metric, anomalies));

        foreach (var anomaly in anomalies)
        {
            await Smtp.SendAsync(
                template: "anomaly",
                subject: $"Anomaly ({anomaly.Key.Metric})",
                preview: "The system detected unusual performance.",
                recipients: Server.Current.Configuration.Administrators,
                parameters: new Dictionary<string, object> {
                    ["title"] = "Anomaly Detected",
                    ["timestamp"] = anomaly.Value.Timestamp.ToString("MMM dd, yyyy @ HH:mm:ss"),
                    ["metric"] = anomaly.Key.Metric,
                    ["condition"] = GetConditionDescription(anomaly.Key.Condition),
                    ["threshold"] = anomaly.Key.Threshold,
                    ["value"] = anomaly.Value.Value
                });
        }

        _notified = DateTime.UtcNow;
    }

    private Dictionary<(string Metric, string Condition, decimal Threshold), (DateTime Timestamp, decimal Value)> GetAnomalies(Metric metric, Dictionary<(string Metric, string Condition, decimal Threshold), (DateTime, decimal)>? anomalies = default)
    {
        anomalies ??= [];

        foreach (var alert in _config.Alerts.Split(";", StringSplitOptions.RemoveEmptyEntries | StringSplitOptions.TrimEntries))
        {
            var expression = Regex.Split(alert.Trim(), "(=|!=|>=|>|<=|<)");
            var target = expression[0].Trim();
            var condition = expression[1].Trim();
            var threshold = decimal.Parse(expression[2].Trim());

            if (!metric.Value.TryGetValue(target, out var value))
            {
                continue;
            }

            switch (condition)
            {
                case "=" when value != threshold:
                case "!=" when value == threshold:
                case ">=" when value < threshold:
                case ">" when value <= threshold:
                case "<=" when value > threshold:
                case "<" when value >= threshold:
                    continue;
            }

            anomalies[(target, condition, threshold)] = (metric.Timestamp, value);
        }

        return anomalies;
    }

    private double GetLoad()
    {
        using var process = Process.GetCurrentProcess();

        var time = DateTime.UtcNow;
        var usage = process.TotalProcessorTime;

        if (!_previousCpuStartTime.HasValue || !_previousTotalProcessorTime.HasValue)
        {
            _previousCpuStartTime = time;
            _previousTotalProcessorTime = usage;
        }

        var du = (usage - _previousTotalProcessorTime.Value).TotalMilliseconds;
        var dt = (time - _previousCpuStartTime.Value).TotalMilliseconds;

        if (dt <= 0)
        {
            return 0;
        }

        _previousCpuStartTime = time;
        _previousTotalProcessorTime = usage;

        return du / (Environment.ProcessorCount * dt) * 100.0;
    }

    private (long WorkingSet64, long PrivateMemorySize64, long VirtualMemorySize64) GetMemory()
    {
        using var process = Process.GetCurrentProcess();

        return (process.WorkingSet64, process.PrivateMemorySize64, process.VirtualMemorySize64);
    }

    private string GetConditionDescription(string condition)
    {
        return condition switch {
            "=" => "Equal to",
            "!=" => "Not equal to",
            ">=" => "Greater than or equal to",
            ">" => "Greater than",
            "<=" => "Less than or equal to",
            "<" => "Less than",
            _ => string.Empty
        };
    }
}

/// <summary>
/// Configurable options for the <see cref="Monitor"/>.
/// </summary>
public class MonitorConfiguration
{
    /// <summary>
    /// A list of conditions upon which to alert the system's administrators.
    /// </summary>
    public string Alerts { get; set; } = string.Empty;

    /// <summary>
    /// The interval at which the system will notify administrators of detected anomalies.
    /// Defaults to 1 minute.
    /// </summary>
    public TimeSpan NotificationInterval { get; set; } = TimeSpan.FromMinutes(1);
}