// Copyright © Spatial Corporation. All rights reserved.

using BenchmarkDotNet.Attributes;
using Spatial.Compute;
using Spatial.Simulation.Components;
using System.Runtime.CompilerServices;

namespace Spatial.Simulation;

/// <summary>
/// Benchmarks for <see cref="Space"/>.
/// </summary>
[GcForce]
[GcServer]
[MemoryDiagnoser]
public class SpaceBenchmarks
{
    /// <summary>
    /// The number of entities to measure.
    /// </summary>
    [Params(1_000, 10_000, 100_000, 1_000_000, 5_000_000)]
    public uint Entities { get; set; }

    private const float _dt = 1f / 60f;
    private const float _damping = 0.99f;
    private const float _maxSpeedSq = 100f * 100f;

    private static readonly Signature _signature = Signature.Combine<Position, Velocity, Rotation>();

    private Space _space = null!;
    private Computer _computer = null!;
    private Query _mono = null!;
    private Query _accelerated = null!;

    /// <summary>
    /// Set up the benchmark.
    /// </summary>
    [GlobalSetup]
    public void Setup() => (_computer = new()).Run();

    /// <summary>
    /// Clean up after all benchmarks.
    /// </summary>
    [GlobalCleanup]
    public void CleanupAll() => _computer.Shutdown();

    /// <summary>
    /// Set up <see cref="Create"/>.
    /// </summary>
    [IterationSetup(Target = nameof(Create))]
    public void SetupCreate() => (_space = new Space()).Reserve(_signature, Entities);

    /// <summary>
    /// Set up physics benchmarks.
    /// </summary>
    [IterationSetup(Targets = [
        nameof(PhysicsMonoLite),
        nameof(PhysicsAcceleratedLite),
        nameof(PhysicsMonoHeavy),
        nameof(PhysicsAcceleratedHeavy)])]
    public void SetupPhysics()
    {
        (_space = new Space()).Reserve(_signature, Entities);

        for (var i = 0; i < Entities; i++)
        {
            _space.Create(_signature);
        }

        var index = 0;
        var seed = new Query().WithAll(_signature);

        _space.Mutate(seed, (Future _, in Entity _, ref Position pos, ref Velocity vel, ref Rotation rot) =>
        {
            pos.X = (index % 1000) * 1.5f;
            pos.Y = (index % 500)  * 2.0f;
            pos.Z = (index % 750)  * 1.2f;

            vel.X = (index % 23) * 4.5f - 50f;
            vel.Y = (index % 17) * 6.0f - 50f;
            vel.Z = (index % 31) * 3.5f - 50f;

            rot.Degrees = index % 360;

            index++;
        });

        _mono = new Query().WithAll(_signature);
        _accelerated = new Query().Accelerate().WithAll(_signature);
    }

    /// <summary>
    /// Clean up after a benchmark.
    /// </summary>
    [IterationCleanup]
    public void Cleanup() => _space.Dispose();

    /// <summary>
    /// Measures <see cref="Space.Create"/> throughput.
    /// </summary>
    [Benchmark]
    [BenchmarkCategory("Create")]
    public void Create()
    {
        for (var i = 0; i < Entities; i++)
        {
            _space.Create(_signature);
        }
    }

    /// <summary>
    /// Measures <see cref="Space.Mutate"/> throughput on a single thread.
    /// Represents steady-state movement: position integration and rotation only.
    /// </summary>
    [Benchmark]
    [BenchmarkCategory("PhysicsLite")]
    public void PhysicsMonoLite()
    {
        _space.Mutate(_mono, (Future _, in Entity _, ref Position pos, ref Velocity vel, ref Rotation rot) => IntegrateLite(ref pos, ref vel, ref rot));
    }

    /// <summary>
    /// Measures <see cref="Space.Mutate"/> throughput with acceleration.
    /// Represents steady-state movement: position integration and rotation only.
    /// </summary>
    [Benchmark]
    [BenchmarkCategory("PhysicsLite")]
    public void PhysicsAcceleratedLite()
    {
        _space.Mutate(_accelerated, (Future _, in Entity _, ref Position pos, ref Velocity vel, ref Rotation rot) => IntegrateLite(ref pos, ref vel, ref rot));
    }

    /// <summary>
    /// Measures <see cref="Space.Mutate"/> throughput on a single thread.
    /// Represents worst-case movement: damping, speed clamp, integration, and rotation.
    /// </summary>
    [Benchmark]
    [BenchmarkCategory("PhysicsHeavy")]
    public void PhysicsMonoHeavy()
    {
        _space.Mutate(_mono, (Future _, in Entity _, ref Position pos, ref Velocity vel, ref Rotation rot) => IntegrateHeavy(ref pos, ref vel, ref rot));
    }

    /// <summary>
    /// Measures <see cref="Space.Mutate"/> throughput with acceleration.
    /// Represents worst-case movement: damping, speed clamp, integration, and rotation.
    /// </summary>
    [Benchmark]
    [BenchmarkCategory("PhysicsHeavy")]
    public void PhysicsAcceleratedHeavy()
    {
        _space.Mutate(_accelerated, (Future _, in Entity _, ref Position pos, ref Velocity vel, ref Rotation rot) => IntegrateHeavy(ref pos, ref vel, ref rot));
    }

    [MethodImpl(MethodImplOptions.AggressiveInlining)]
    private static void IntegrateLite(ref Position pos, ref Velocity vel, ref Rotation rot)
    {
        pos.X += vel.X * _dt;
        pos.Y += vel.Y * _dt;
        pos.Z += vel.Z * _dt;

        rot.Degrees = (rot.Degrees + 1) % 360;
    }

    [MethodImpl(MethodImplOptions.AggressiveInlining)]
    private static void IntegrateHeavy(ref Position pos, ref Velocity vel, ref Rotation rot)
    {
        vel.X *= _damping;
        vel.Y *= _damping;
        vel.Z *= _damping;

        var speedSq = vel.X * vel.X + vel.Y * vel.Y + vel.Z * vel.Z;

        if (speedSq > _maxSpeedSq)
        {
            var inv = MathF.ReciprocalSqrtEstimate(speedSq) * MathF.Sqrt(_maxSpeedSq);

            vel.X *= inv;
            vel.Y *= inv;
            vel.Z *= inv;
        }

        pos.X += vel.X * _dt;
        pos.Y += vel.Y * _dt;
        pos.Z += vel.Z * _dt;

        rot.Degrees = (rot.Degrees + 1) % 360;
    }
}