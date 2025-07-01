// Copyright © Spatial Corporation. All rights reserved.

using BenchmarkDotNet.Attributes;
using Spatial.Simulation.Components;

namespace Spatial.Simulation;

/// <summary>
/// Benchmarks for <see cref="Space"/>.
/// </summary>
[GcForce]
[GcServer]
[MemoryDiagnoser]
public class SpaceBenchmarks
{
    private const int _entities = 1000 * 1000 * 5;

    private Space _space = null!;
    private Signature _signature;
    private Query _query;

    /// <summary>
    /// An entity's component count.
    /// </summary>
    [Params(0, 1, 2, 3)]
    public int Components { get; set; }

    /// <summary>
    /// Setup a benchmark.
    /// </summary>
    [GlobalSetup]
    public void Setup()
    {
        _signature = Components switch
        {
            1 => Signature.Of<Position>(),
            2 => Signature.Combine<Position, Velocity>(),
            3 => Signature.Combine<Position, Velocity, Rotation>(),
            _ => Signature.Empty
        };
    }

    /// <summary>
    /// Setup <see cref="Create"/>.
    /// </summary>
    [IterationSetup(Target = nameof(Create))]
    public void SetupCreate()
    {
        (_space = new Space()).Reserve(_signature, _entities);
    }

    /// <summary>
    /// Setup <see cref="MutateMono"/>.
    /// </summary>
    [IterationSetup(Target = nameof(MutateMono))]
    public void SetupMutateMono()
    {
        (_space = new Space()).Reserve(_signature, _entities);
        _query = new Query().WithAll(_signature);

        CreateImpl();
    }

    /// <summary>
    /// Setup <see cref="MutateAccelerated"/>.
    /// </summary>
    [IterationSetup(Target = nameof(MutateAccelerated))]
    public void SetupMutateAccelerated()
    {
        (_space = new Space()).Reserve(_signature, _entities);
        _query = new Query().Parallel().WithAll(_signature);

        CreateImpl();
    }

    /// <summary>
    /// Measure <see cref="Space.Create"/>.
    /// </summary>
    [BenchmarkCategory("Create")]
    [Benchmark(OperationsPerInvoke = _entities)]
    public void Create()
    {
        CreateImpl();
    }

    /// <summary>
    /// Measure <see cref="Space.Mutate"/>.
    /// </summary>
    [BenchmarkCategory("MutateMono")]
    [Benchmark(OperationsPerInvoke = _entities)]
    public void MutateMono()
    {
        MutateImpl();
    }

    /// <summary>
    /// Measure <see cref="Space.Mutate"/>.
    /// </summary>
    [BenchmarkCategory("MutateAccelerated")]
    [Benchmark(OperationsPerInvoke = _entities)]
    public void MutateAccelerated()
    {
        MutateImpl();
    }

    /// <summary>
    /// Cleanup after a benchmark.
    /// </summary>
    [IterationCleanup]
    public void Cleanup()
    {
        _space.Dispose();
    }

    private void CreateImpl()
    {
        for (var i = 0; i < _entities; i++)
        {
            _space.Create(_signature);
        }
    }

    private void MutateImpl()
    {
        switch (Components)
        {
            case 0:
                _space.Mutate(_query, (Future future, in Entity entity) => {});
                break;
            case 1:
                _space.Mutate(_query, (Future future, in Entity entity, ref Position position) => {
                    position.X++;
                    position.Y++;
                    position.Z++;
                });

                break;
            case 2:
                _space.Mutate(_query, (Future future, in Entity entity, ref Position position, ref Velocity velocity) => {
                    position.X += velocity.X;
                    position.Y += velocity.Y;
                    position.Z += velocity.Z;
                });

                break;
            case 3:
                _space.Mutate(_query, (Future future, in Entity entity, ref Position position, ref Velocity velocity, ref Rotation rotation) => {
                    position.X += velocity.X;
                    position.Y += velocity.Y;
                    position.Z += velocity.Z;
                    rotation.Degrees = (rotation.Degrees + 1) % 360;
                });
                
                break;
        }
    }
}