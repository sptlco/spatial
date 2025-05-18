// Copyright © Spatial. All rights reserved.

using Ignite.Components;
using Ignite.Contracts;
using Ignite.Models.Objects;
using Serilog;
using Spatial.Mathematics;
using Spatial.Simulation;
using Spatial.Structures;
using System;
using System.Collections.Concurrent;
using System.Collections.Generic;

namespace Ignite.Models;

/// <summary>
/// A layered, two-dimensional grid covering a <see cref="Map"/>.
/// </summary>
public class Grid
{
    /// <summary>
    /// The size of a <see cref="Grid"/>.
    /// </summary>
    public const int SIZE = 512;

    /// <summary>
    /// The number of chunks along each axis of a <see cref="Grid"/>.
    /// </summary>
    public const int CHUNK_DIMENSION = 10;

    /// <summary>
    /// The size of a <see cref="Chunk"/>.
    /// </summary>
    public const float CHUNK_SIZE = SIZE / CHUNK_DIMENSION;

    /// <summary>
    /// Half the size of a <see cref="Chunk"/>.
    /// </summary>
    public const float CHUNK_HALF_SIZE = CHUNK_SIZE / 2.0F;

    /// <summary>
    /// The size of a spatial <see cref="Grid"/> cell.
    /// </summary>
    public const int CELL_SIZE = 8;

    /// <summary>
    /// The number of cells along each axis of a <see cref="Grid"/>.
    /// </summary>
    public const int CELL_DIMENSION = SIZE / CELL_SIZE;

    private readonly Map _map;
    private readonly Quad _bounds;
    private readonly Point2D _size;
    private readonly Point2D _scale2d;
    private readonly Point2D _offset;
    private readonly float _scale;

    private readonly Entity[] _chunks;
    private readonly ConcurrentDictionary<(byte, byte), ConcurrentHashSet<uint>> _cells;

    /// <summary>
    /// Create a new <see cref="Grid"/>.
    /// </summary>
    /// <param name="map">The <see cref="Map"/> bound to the <see cref="Grid"/>.</param>
    public Grid(Map map)
    {
        _map = map;
        _bounds = new Quad(_map.Data.View.StartX, _map.Data.View.StartY, _map.Data.View.EndX, _map.Data.View.EndY);
        _size = new Point2D(_bounds.Right - _bounds.Left + 1, _bounds.Bottom - _bounds.Top + 1);
        _scale2d = new Point2D(SIZE / _size.X, SIZE / _size.Y);
        _offset = new Point2D(_bounds.Left, _bounds.Top);
        _scale = _map.Data.View.MiniMapScale;

        _chunks = new Entity[CHUNK_DIMENSION * CHUNK_DIMENSION];
        _cells = [];

        CreateChunks();
    }

    /// <summary>
    /// Set the position of an <see cref="Entity"/>.
    /// </summary>
    /// <param name="entity">The <see cref="Entity"/> to move.</param>
    /// <param name="position">The entity's position.</param>
    public void Set(in Entity entity, in Transform position)
    {
        var normal = Normalize(position);

        _cells.GetOrAdd(((byte) (normal.X / CELL_SIZE), (byte) (normal.Y / CELL_SIZE)), []).Add(entity);
    }

    /// <summary>
    /// Compute a <see cref="Grid"/> view, enumerating chunks within sight range of a <see cref="Transform"/>.
    /// </summary>
    /// <param name="position">A <see cref="Transform"/>.</param>
    /// <returns>A list of chunks.</returns>
    public IEnumerable<Entity> View(in Transform position)
    {
        return Query(
            position: position,
            radius: _map.Data.Info.Sight + (DenormalizeX(_bounds.Right) - DenormalizeX(_bounds.Left)) / CHUNK_DIMENSION / 2.0F,
            type: ObjectType.Chunk);
    }

    /// <summary>
    /// Query the <see cref="Grid"/>.
    /// </summary>
    /// <param name="position">The position to query.</param>
    /// <param name="radius">The radius to query.</param>
    /// <returns>A list of entities in the space.</returns>
    public IEnumerable<Entity> Query(in Transform position, in float radius)
    {
        return Query(position, radius, null);
    }

    /// <summary>
    /// Query the <see cref="Grid"/>.
    /// </summary>
    /// <param name="position">The position to query.</param>
    /// <param name="radius">The radius to query.</param>
    /// <param name="type">The <see cref="ObjectType"/> to query.</param>
    /// <returns>A list of entities in the space.</returns>
    public IEnumerable<Entity> Query(in Transform position, in float radius, ObjectType type, Func<Entity, bool>? filter = null)
    {
        return Query(position.X, position.Y, radius, type, filter);
    }

    /// <summary>
    /// Query the <see cref="Grid"/>.
    /// </summary>
    /// <param name="x">An X-coordinate.</param>
    /// <param name="y">A Y-coordinate.</param>
    /// <param name="radius">The radius to query.</param>
    /// <param name="type">The <see cref="ObjectType"/> to query.</param>
    /// <returns>A list of entities in the space.</returns>
    public IEnumerable<Entity> Query(in float x, in float y, in float radius, ObjectType type, Func<Entity, bool>? filter = null)
    {
        return Query(x, y, radius, entity => _map.Space.Has<Tag>(entity) && _map.Space.Get<Tag>(entity).Type == type && (filter?.Invoke(entity) ?? true));
    }

    /// <summary>
    /// Query the <see cref="Grid"/>.
    /// </summary>
    /// <param name="position">The position to query.</param>
    /// <param name="radius">The radius to query.</param>
    /// <param name="bias">A value added to the normalized position.</param>
    /// <param name="type">The <see cref="ObjectType"/> to query.</param>
    /// <returns>A list of entities in the space.</returns>
    public IEnumerable<Entity> Query(in Transform position, in float radius, Func<Entity, bool>? filter = null)
    {
        return Query(position.X, position.Y, radius, filter);
    }

    /// <summary>
    /// Query the <see cref="Grid"/>.
    /// </summary>
    /// <param name="px">The X-coordinate to query.</param>
    /// <param name="px">The Y-coordinate to query.</param>
    /// <param name="radius">The radius to query.</param>
    /// <param name="bias">A value added to the normalized position.</param>
    /// <param name="type">The <see cref="ObjectType"/> to query.</param>
    /// <returns>A list of entities in the space.</returns>
    public IEnumerable<Entity> Query(float px, float py, float radius, Func<Entity, bool>? filter = null)
    {
        var chunks = new HashSet<uint>();

        var left = (byte) (Math.Clamp(NormalizeX(px - radius), 0, SIZE - 1) / CELL_SIZE);
        var top = (byte) (Math.Clamp(NormalizeY(py + radius), 0, SIZE - 1) / CELL_SIZE);
        var right = (byte) (Math.Clamp(NormalizeX(px + radius), 0, SIZE - 1) / CELL_SIZE);
        var bottom = (byte) (Math.Clamp(NormalizeY(py - radius), 0, SIZE - 1) / CELL_SIZE);

        for (byte x = left; x <= right; x++)
        {
            for (byte y = top; y <= bottom; y++)
            {
                var chunk = ChunkAt((int) (x * CELL_SIZE / CHUNK_SIZE), (int) (y * CELL_SIZE / CHUNK_SIZE));

                if ((filter == default || filter(chunk)) && !chunks.Contains(chunk))
                {
                    chunks.Add(chunk);
                    yield return chunk;
                }

                if (_cells.TryGetValue((x, y), out var cell))
                {
                    foreach (var entity in cell)
                    {
                        if (filter == default || filter(entity))
                        {
                            yield return entity;
                        }
                    }
                }
            }
        }
    }

    /// <summary>
    /// Clear the <see cref="Grid"/>.
    /// </summary>
    public void Clear()
    {
        _cells.Clear();
    }

    /// <summary>
    /// Get whether or not a <see cref="Chunk"/> contains an <see cref="Entity"/>.
    /// </summary>
    /// <param name="chunk">A <see cref="Chunk"/>.</param>
    /// <param name="entity">An <see cref="Entity"/>.</param>
    /// <returns>Whether or not the <see cref="Chunk"/> contains the <see cref="Entity"/>.</returns>
    public bool Contains(Entity chunk, Entity entity)
    {
        return ChunkAt(entity) == chunk;
    }

    /// <summary>
    /// Get the <see cref="Chunk"/> at an entity's location.
    /// </summary>
    /// <param name="entity">An <see cref="Entity"/>.</param>
    /// <returns>A <see cref="Chunk"/>.</returns>
    public Entity ChunkAt(Entity entity)
    {
        return ChunkAt(_map.Space.Get<Transform>(entity));
    }

    /// <summary>
    /// Get the <see cref="Chunk"/> at a location.
    /// </summary>
    /// <param name="transform">A <see cref="Transform"/>.</param>
    /// <returns>A <see cref="Chunk"/>.</returns>
    public Entity ChunkAt(in Transform transform)
    {
        return ChunkAt(transform.X, transform.Y);
    }

    /// <summary>
    /// Get the <see cref="Chunk"/> at a location.
    /// </summary>
    /// <param name="x">An X-coordinate.</param>
    /// <param name="y">A Y-coordinate.</param>
    /// <returns>A <see cref="Chunk"/>.</returns>
    public Entity ChunkAt(in float x, in float y)
    {
        var normal = Normalize(x, y);

        return ChunkAt((int) (normal.X / CHUNK_SIZE), (int) (normal.Y / CHUNK_SIZE));
    }

    /// <summary>
    /// Get the <see cref="Chunk"/> at a location.
    /// </summary>
    /// <param name="x">The chunk's X-coordinate.</param>
    /// <param name="y">The chunk's Y-coordinate.</param>
    /// <returns>A <see cref="Chunk"/>.</returns>
    public Entity ChunkAt(in int x, in int y)
    {
        if (x < 0 || x >= CHUNK_DIMENSION || y < 0 || y >= CHUNK_DIMENSION)
        {
            return Entity.Null;
        }

        return _chunks[(y * CHUNK_DIMENSION) + x];
    }

    private void CreateChunks()
    {
        for (var x = 0; x < CHUNK_DIMENSION; x++)
        {
            for (var y = 0; y < CHUNK_DIMENSION; y++)
            {
                var chunk = _map.CreatePlainObject<ChunkRef>(ObjectType.Chunk);

                var cx = CHUNK_HALF_SIZE + (x * CHUNK_SIZE);
                var cy = CHUNK_HALF_SIZE + (y * CHUNK_SIZE);

                chunk.Add(new Chunk(x, y));
                chunk.Add(new Transform(DenormalizeX(cx), DenormalizeY(cy)));
                chunk.Add(new Disabled());

                _chunks[(y * CHUNK_DIMENSION) + x] = chunk.UID;
            }
        }
    }

    private Transform Normalize(in Transform transform)
    {
        return (Transform) Normalize(transform.X, transform.Y) with { R = transform.R };
    }

    private Point2D Normalize(in float x, in float y)
    {
        return new Point2D(NormalizeX(x), NormalizeY(y));
    }

    private float NormalizeX(float x)
    {
        return (x * Constants.px / _scale - _offset.X) * _scale2d.X;
    }

    private float NormalizeY(float y)
    {
        return (SIZE - y * Constants.px / _scale - _offset.Y) * _scale2d.Y;
    }

    private float DenormalizeX(float x)
    {
        return ((x / _scale2d.X) + _offset.X) * _scale / Constants.px;
    }

    private float DenormalizeY(float y)
    {
        return (SIZE - ((y / _scale2d.Y) + _offset.Y)) * _scale / Constants.px;
    }
}