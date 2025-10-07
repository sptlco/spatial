// Copyright © Spatial Corporation. All rights reserved.

using System.Diagnostics.CodeAnalysis;

namespace Spatial.Structures;

/// <summary>
/// A thread-safe <see cref="Queue{T}"/>.
/// </summary>
/// <typeparam name="T">The type of objects in the <see cref="InterlockedQueue{T}"/>.</typeparam>
public sealed class InterlockedQueue<T>
{
    private readonly Queue<T> _queue;
    private readonly Lock _lock = new();

    /// <summary>
    /// Create a new <see cref="InterlockedQueue{T}"/>.
    /// </summary>
    public InterlockedQueue()
    {
        _queue = new Queue<T>();
    }

    /// <summary>
    /// Create a new <see cref="InterlockedQueue{T}"/>.
    /// </summary>
    /// <param name="collection">A collection to initialize the <see cref="InterlockedQueue{T}"/> with.</param>
    public InterlockedQueue(IEnumerable<T> collection)
    {
        _queue = new Queue<T>(collection);
    }

    /// <summary>
    /// Get the number of elements in the <see cref="InterlockedQueue{T}"/>.
    /// </summary>
    public int Count { get { lock (_lock) { return _queue.Count; } } }

    /// <summary>
    /// Get whether or not the <see cref="InterlockedQueue{T}"/> is empty.
    /// </summary>
    public bool Empty => Count == 0;

    /// <summary>
    /// Dequeue an item from the <see cref="InterlockedQueue{T}"/>.
    /// </summary>
    /// <param name="value">The item that was dequeued.</param>
    /// <returns>Whether or not an item was dequeued.</returns>
    public bool TryDequeue([MaybeNullWhen(false)] out T value)
    {
        lock (_lock)
        {
            if (_queue.Count > 0)
            {
                value = _queue.Dequeue();
                return true;
            }

            value = default;
            return false;
        }
    }

    /// <summary>
    /// Enqueue an item.
    /// </summary>
    /// <param name="value">The item to enqueue.</param>
    public void Enqueue(T value)
    {
        lock (_lock)
        {
            _queue.Enqueue(value);
        }
    }

    /// <summary>
    /// Clear the <see cref="InterlockedQueue{T}"/>.
    /// </summary>
    public void Clear()
    {
        lock (_lock)
        {
            _queue.Clear();
        }
    }
}