// Copyright © Spatial Corporation. All rights reserved.

using Spatial.Compute.Acceleration;
using Spatial.Compute.Commands;
using Spatial.Structures;
using System.Diagnostics.CodeAnalysis;

namespace Spatial.Compute;

/// <summary>
/// An opportunistic worker that pulls and executes <see cref="CommandJob"/>s
/// from the <see cref="Computer"/> or another <see cref="Agent"/>.
/// </summary>
internal class Agent : IDisposable
{
    private readonly int _id;
    private readonly Computer _computer;
    private readonly Thread _thread;
    private readonly CancellationTokenSource _cts;
    private readonly InterlockedQueue<CommandJob> _queue;
    private readonly ManualResetEventSlim _signal;
    private uint _next;

    /// <summary>
    /// Create a new <see cref="Agent"/>.
    /// </summary>
    /// <param name="computer">The agent's <see cref="Computer"/>.</param>
    /// <param name="id">The agent's identification number.</param>
    public Agent(Computer computer, int id)
    {
        _id = id;
        _computer = computer;
        _thread = CreateThread();
        _cts = new();
        _queue = new();
        _signal = new ManualResetEventSlim(false);
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
    /// Wake the <see cref="Agent"/>.
    /// </summary>
    public void Wake()
    {
        _signal.Set();
    }

    /// <summary>
    /// Dispose of the <see cref="Agent"/>.
    /// </summary>
    public void Dispose()
    {
        _cts.Cancel();
        _signal.Set();
        _thread.Join();
        _cts.Dispose();
        _queue.Clear();
        _signal.Dispose();
    }

    private void Work()
    {
        while (!_cts.IsCancellationRequested)
        {
            if (Fetch(out var job))
            {
                Execute(job);
                continue;
            }

            try
            {
                _signal.Wait(_cts.Token);
            }
            catch (OperationCanceledException)
            {
                break;
            }
            
            _signal.Reset();
        }
    }

    private bool Fetch([MaybeNullWhen(false)] out CommandJob job)
    {
        return _queue.TryDequeue(out job) || TrySteal(out job);
    }

    private bool TrySteal([MaybeNullWhen(false)] out CommandJob job)
    {
        job = default;

        if (_computer.Agents.Length <= 1)
        {
            return false;
        }

        var failures = 0;
        var yields = 0;

        while (true)
        {
            var agent = _next++ % (uint) _computer.Agents.Length;

            if (agent != _id && _computer.Agents[agent].Queue.TryDequeue(out job))
            {
                return true;
            }

            if (++failures >= 2 * (_computer.Agents.Length - 1))
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

    private void Execute(CommandJob job)
    {
        job.Executed = Time.Now;
        job.Status = JobStatus.Running;

        try
        {
            new Driver(job).Run();

            job.Status = JobStatus.Completed;
        }
        catch (Exception e)
        {
            job.Status = JobStatus.Failed;

            ERROR(e, "{job} failed.", job.GetType().Name);
        }
        finally
        {
            _computer.Finalize(job.Id);
        }
    }

    private Thread CreateThread()
    {
        return new Thread(Work) {
            Name = Constants.AgentThreadName,
            Priority = ThreadPriority.Highest,
        };
    }
}