// Copyright © Spatial. All rights reserved.

using Spatial.Extensions;
using System.Collections.Concurrent;

namespace Spatial.Compute.Jobs;

/// <summary>
/// A <see cref="Job"/> execution graph.
/// </summary>
internal class Graph : IDisposable
{
    private static readonly ConcurrentBag<Graph> _pool = [];

    private Graph()
    {
    }
    
    /// <summary>
    /// The graph's handle.
    /// </summary>
    public JobHandle Handle { get; set; } = null!;

    /// <summary>
    /// The jobs to execute.
    /// </summary>
    public List<Job> Jobs { get; private set; } = [];

    /// <summary>
    /// A directed acyclic graph (DAG) of dependencies.
    /// </summary>
    public Dictionary<Job, List<Job>> Dependencies { get; } = [];

    /// <summary>
    /// A directed acyclic graph (DAG) of dependants.
    /// </summary>
    public Dictionary<Job, List<Job>> Dependants { get; } = [];

    /// <summary>
    /// Create a new <see cref="Graph"/>.
    /// </summary>
    /// <returns>A <see cref="Graph"/>.</returns>
    internal static Graph Create()
    {
        // if (_pool.TryTake(out var graph))
        // {
        //     return graph;
        // }

        return new();
    }
    
    /// <summary>
    /// Add a <see cref="Job"/> to the graph.
    /// </summary>
    /// <param name="job">The <see cref="Job"/> to add.</param>
    /// <param name="dependencies">A list of dependencies.</param>
    public Graph Add(Job job, params Job[] dependencies)
    {
        job.Graph = this;
        
        Jobs.Add(job);

        foreach (var dependency in dependencies)
        {
            Dependencies.GetOrAdd(job, _ => []).Add(dependency);
            Dependants.GetOrAdd(dependency, _ => []).Add(job);
        }

        return this;
    }

    /// <summary>
    /// Topologically sort the <see cref="Graph"/>.
    /// </summary>
    public void Sort()
    {
        // Read more about Khan's topological sorting algorithm here:
        // https://www.geeksforgeeks.org/topological-sorting-indegree-based-solution

        var count = Jobs.Count;
        var dependants = new Dictionary<Job, int>();
        var queue = new Queue<Job>();
        
        foreach (var job in Jobs)
        {
            if ((dependants[job] = Dependants.GetValueOrDefault(job)?.Count ?? 0) <= 0)
            {
                queue.Enqueue(job);
            }
        }

        Jobs.Clear();

        while (queue.TryDequeue(out var job))
        {
            Jobs.Insert(0, job);
            
            if (Dependencies.TryGetValue(job, out var dependencies))
            {
                foreach (var dependency in dependencies)
                {
                    if (--dependants[dependency] <= 0)
                    {
                        queue.Enqueue(dependency);
                    }
                }
            }
        }

        dependants.Clear();
        queue.Clear();

        if (Jobs.Count != count)
        {
            throw new InvalidOperationException("Cyclical graph detected.");
        }
    }

    /// <summary>
    /// Dispose of the <see cref="Graph"/>.
    /// </summary>
    public void Dispose()
    {
        Handle = null!;
        Jobs.Clear();
        Dependencies.Clear();
        Dependants.Clear();

        // _pool.Add(this);
    }
}