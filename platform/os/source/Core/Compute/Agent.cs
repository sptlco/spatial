// Copyright © Spatial. All rights reserved.

using Serilog;
using Spatial.Compute.Jobs;
using Spatial.Compute.Jobs.Commands;
using Spatial.Structures;
using System.Diagnostics.CodeAnalysis;

namespace Spatial.Compute;

/// <summary>
/// An opportunistic worker that pulls and executes <see cref="CommandJob"/>s
/// from the <see cref="Processor"/> or another <see cref="Agent"/>.
/// </summary>
internal class Agent : IDisposable
{
    private readonly int _id;
    private readonly Thread _thread;
    private readonly CancellationTokenSource _cts;
    private readonly InterlockedQueue<CommandJob> _queue;
    private uint _next;

    /// <summary>
    /// Create a new <see cref="Agent"/>.
    /// </summary>
    /// <param name="id">The agent's identification number.</param>
    public Agent(int id)
    {
        _id = id;
        _thread = CreateThread();
        _cts = new();
        _queue = new();
    }

    /// <summary>
    /// The agent's work queue.
    /// </summary>
    public InterlockedQueue<CommandJob> Queue => _queue;

    /// <summary>
    /// Run the <see cref="Agent"/>.
    /// </summary>
    public void Run()
    {
        _thread.Start();
    }

    /// <summary>
    /// Dispose of the <see cref="Agent"/>.
    /// </summary>
    public void Dispose()
    {
        _cts.Cancel();
        _thread.Join();
        _cts.Dispose();
        _queue.Clear();
    }

    private void Work()
    {
        while (!_cts.IsCancellationRequested)
        {
            if (!Fetch(out var job))
            {
                Thread.SpinWait(50);
                Thread.Yield();
                continue;
            }

            try
            {
                Execute(job);
            }
            catch
            {
                // ...
            }
        }
    }

    private bool Fetch([MaybeNullWhen(false)] out CommandJob job)
    {
        return _queue.TryDequeue(out job) || TrySteal(out job);
    }

    private bool TrySteal([MaybeNullWhen(false)] out CommandJob job)
    {
        job = default;

        if (Processor.Agents.Length <= 1)
        {
            return false;
        }

        var failures = 0;
        var yields = 0;

        while (true)
        {
            var agent = _next++ % (uint) Processor.Agents.Length;

            if (agent != _id && Processor.Agents[agent].Queue.TryDequeue(out job))
            {
                return true;
            }

            if (++failures >= 2 * (Processor.Agents.Length - 1))
            {
                Thread.Yield();

                if (++yields >= Constants.AgentMaxYields)
                {
                    break;
                }
            }
        }

        return false;
    }

    private static void Execute(CommandJob job)
    {
        job.Status = JobStatus.Running;

        try
        {
            job.Execute();
            job.Status = JobStatus.Complete;
        }
        catch (Exception e)
        {
            job.Status = JobStatus.Failed;

            Log.Error(e, "{job} failed", job.GetType().Name);
        }
        finally
        {
            Processor.Finalize(job.Id);
        }
    }

    private Thread CreateThread()
    {
        return new Thread(Work)
        {
            Name = Constants.AgentThreadName,
            IsBackground = true
        };
    }
}