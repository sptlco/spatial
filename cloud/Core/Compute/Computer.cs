// Copyright © Spatial Corporation. All rights reserved.

using Spatial.Compute.Acceleration;
using Spatial.Compute.Commands;
using System.Collections.Concurrent;
using System.Runtime.CompilerServices;

namespace Spatial.Compute;

/// <summary>
/// A low-level abstraction that provides access to the system's 
/// central processing unit (CPU) and graphics processing unit (GPU).
/// </summary>
public class Computer
{
    private static Computer _instance;

    private Agent[] _agents;
    private ConcurrentDictionary<string, Job> _jobs;
    private ConcurrentDictionary<Job, StrongBox<int>> _dependencies;
    private int _running;
    private uint _next;

    /// <summary>
    /// Create a new <see cref="Computer"/>.
    /// </summary>
    public Computer()
    {
        _instance = this;
        _agents = new Agent[Environment.ProcessorCount];
    }

    /// <summary>
    /// The current <see cref="Computer"/>.
    /// </summary>
    public static Computer Current => _instance;

    /// <summary>
    /// The agents of the <see cref="Computer"/>.
    /// </summary>
    internal Agent[] Agents => _agents;

    /// <summary>
    /// Run the <see cref="Computer"/>.
    /// </summary>
    public void Run()
    {
        if (Interlocked.Exchange(ref _running, 1) != 0)
        {
            return;
        }

        _jobs = [];
        _dependencies = [];

        for (var i = 0; i < _agents.Length; i++)
        {
            _agents[i] = new(this, i);
        }

        for (var i = 0; i < _agents.Length; i++)
        {
            _agents[i].Run();
        }
    }

    /// <summary>
    /// Shutdown the <see cref="Computer"/>.
    /// </summary>
    public void Shutdown()
    {
        if (Interlocked.Exchange(ref _running, 0) != 1)
        {
            return;
        }

        for (var i = 0; i < _agents.Length; i++)
        {
            _agents[i].Dispose();
        }

        _agents = default!;
    }

    /// <summary>
    /// Dispatch a <see cref="Graph"/> to the <see cref="Computer"/>.
    /// </summary>
    /// <param name="graph">A <see cref="Job"/> execution <see cref="Graph"/>.</param>
    /// <returns>A <see cref="Handle"/>.</returns>
    internal Handle Dispatch(Graph graph)
    {
        // First, topologically sort the graph using Khan's algorithm
        // under the hood, ensuring there are no circular dependencies.

        if (graph.Dependencies.Count > 0)
        {
            graph.Sort();

            // To ensure that all dependencies are accounted for before 
            // jobs are submitted, we add them here.
            
            foreach (var (jobId, dependencies) in graph.Dependencies)
            {
                _dependencies[jobId] = new(dependencies.Count);
            }
        }
        
        // Finally, submit the jobs for execution.
        // Note, they won't actually be scheduled yet unless their 
        // dependencies have all been met.

        graph.Handle = Handle.Create(graph.Jobs.Count);
        graph.Jobs.ForEach(Submit);

        return graph.Handle;
    }

    /// <summary>
    /// Finalize a <see cref="Job"/> that was executed.
    /// </summary>
    /// <param name="jobId">The <see cref="Job"/> to finalize.</param>
    public void Finalize(string jobId)
    {
        if (_jobs.TryRemove(jobId, out var job))
        {
            switch (job)
            {
                case BatchJob batch1D:
                    if (batch1D.Parent.Complete(batch1D.Size))
                    {
                        Finalize(batch1D.Parent.Id);
                    }

                    break;
                case Batch2DJob batch2D:
                    if (batch2D.Parent.Complete(batch2D.Size))
                    {
                        Finalize(batch2D.Parent.Id);
                    }

                    break;
                default:
                    // Notify dependents of this job (jobs that cannot run before this 
                    // job is executed) that the job has been executed, so that they can run.
                    
                    if (job.Graph.Dependants.TryGetValue(job, out var dependants))
                    {
                        foreach (var dependant in dependants)
                        {
                            if (Interlocked.Decrement(ref _dependencies[dependant].Value) <= 0 && _dependencies.TryRemove(dependant, out _))
                            {
                                Process(dependant);
                            }
                        }
                    }
                    
                    job.Graph.Handle.Signal();
                    break;
            }

            job.Dispose();
        }
    }

    private void Submit(Job job)
    {
        job.Status = JobStatus.Submitted;
        
        _jobs[job.Id] = job;

        if (job is not BatchJob and not Batch2DJob)
        {
            if (_dependencies.TryGetValue(job, out var box) && box.Value > 0)
            {
                // If the job has dependencies that have not yet been executed,
                // defer the job's execution.
                
                return;
            }
        }
        
        Process(job);
    }

    private void Process(Job job)
    {
        switch (job)
        {
            case ParallelForJob job1D:
                for (var i = 0; i < Math.Ceiling(job1D.Iterations / (double) job1D.BatchSize); i++)
                {
                    var start = i * job1D.BatchSize;
                    var end = Math.Min(start + job1D.BatchSize, job1D.Iterations);
                    var batch = BatchJob.Create(job1D, start, end);

                    Submit(batch);
                }

                break;
            case ParallelFor2DJob job2D:
                var batchesX = Math.Ceiling(job2D.Width / (double) job2D.BatchSizeX);
                var batchesY = Math.Ceiling(job2D.Height / (double) job2D.BatchSizeY);

                for (var bx = 0; bx < batchesX; bx++)
                {
                    for (var by = 0; by < batchesY; by++)
                    {
                        var startX = bx * job2D.BatchSizeX;
                        var endX = Math.Min(startX + job2D.BatchSizeX, job2D.Width);
                        var startY = by * job2D.BatchSizeY;
                        var endY = Math.Min(startY + job2D.BatchSizeY, job2D.Height);
                        var batch = Batch2DJob.Create(job2D, startX, endX, startY, endY);

                        Submit(batch);
                    }
                }

                break;
            case CommandJob command:
                Execute(command);
                break;
        }
    }

    private void Execute(CommandJob job)
    {
        job.Status = JobStatus.Scheduled;

        var agent = _agents[Interlocked.Increment(ref _next) % _agents.Length];

        agent.Queue.Enqueue(job);
        agent.Wake();
    }
}