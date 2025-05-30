// Copyright © Spatial. All rights reserved.

using Ignite.Components;
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
    }

    /// <summary>
    /// Set the position of an <see cref="Entity"/>.
    /// </summary>
    /// <param name="entity">The <see cref="Entity"/> to move.</param>
    /// <param name="position">The entity's position.</param>
    public void Set(in Entity entity, in Transform position)
    {
        _cells.GetOrAdd(((byte) (NormalizeX(position.X) / CELL_SIZE), (byte) (NormalizeY(position.Y) / CELL_SIZE)), []).Add(entity);
    }

    /// <summary>
    /// Query the <see cref="Grid"/>.
    /// </summary>
    /// <param name="px">The X-coordinate to query.</param>
    /// <param name="px">The Y-coordinate to query.</param>
    /// <param name="radius">The radius to query.</param>
    /// <returns>A list of entities in the space.</returns>
    public IEnumerable<Entity> Query(float px, float py, float radius)
    {
        var left = (byte) (Math.Clamp(NormalizeX(px - radius), 0, SIZE - 1) / CELL_SIZE);
        var top = (byte) (Math.Clamp(NormalizeY(py + radius), 0, SIZE - 1) / CELL_SIZE);
        var right = (byte) (Math.Clamp(NormalizeX(px + radius), 0, SIZE - 1) / CELL_SIZE);
        var bottom = (byte) (Math.Clamp(NormalizeY(py - radius), 0, SIZE - 1) / CELL_SIZE);

        for (byte x = left; x <= right; x++)
        {
            for (byte y = top; y <= bottom; y++)
            {
                if (_cells.TryGetValue((x, y), out var cell))
                {
                    foreach (var entity in cell)
                    {
                        yield return entity;
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