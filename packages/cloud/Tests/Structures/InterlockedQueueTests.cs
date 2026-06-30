// Copyright © Spatial Corporation. All rights reserved.

namespace Spatial.Structures;

/// <summary>
/// Tests for <see cref="InterlockedQueue{T}"/>.
/// </summary>
public class InterlockedQueueTests
{
    /// <summary>
    /// Create an empty <see cref="InterlockedQueue{T}"/>.
    /// </summary>
    [Fact]
    [Trait("Category", "Unit")]
    public void TestConstructorEmpty()
    {
        var queue = new InterlockedQueue<int>();

        Assert.Equal(0, queue.Count);
        Assert.True(queue.Empty);
    }

    /// <summary>
    /// Create a <see cref="InterlockedQueue{T}"/> from a collection.
    /// </summary>
    [Fact]
    [Trait("Category", "Unit")]
    public void TestConstructorFromCollection()
    {
        var queue = new InterlockedQueue<int>([1, 2, 3]);

        Assert.Equal(3, queue.Count);
        Assert.False(queue.Empty);
    }

    /// <summary>
    /// Get the number of elements in the <see cref="InterlockedQueue{T}"/>.
    /// </summary>
    [Fact]
    [Trait("Category", "Unit")]
    public void TestCount()
    {
        var queue = new InterlockedQueue<int>();

        queue.Enqueue(1);
        queue.Enqueue(2);

        Assert.Equal(2, queue.Count);
    }

    /// <summary>
    /// Get whether or not the <see cref="InterlockedQueue{T}"/> is empty.
    /// </summary>
    [Fact]
    [Trait("Category", "Unit")]
    public void TestEmpty()
    {
        var queue = new InterlockedQueue<int>();

        Assert.True(queue.Empty);

        queue.Enqueue(1);

        Assert.False(queue.Empty);
    }

    /// <summary>
    /// Dequeue an item from a non-empty <see cref="InterlockedQueue{T}"/>.
    /// </summary>
    [Fact]
    [Trait("Category", "Unit")]
    public void TestTryDequeueSucceeds()
    {
        var queue = new InterlockedQueue<int>([1, 2, 3]);

        var result = queue.TryDequeue(out var value);

        Assert.True(result);
        Assert.Equal(1, value);
        Assert.Equal(2, queue.Count);
    }

    /// <summary>
    /// Dequeue an item from an empty <see cref="InterlockedQueue{T}"/>.
    /// </summary>
    [Fact]
    [Trait("Category", "Unit")]
    public void TestTryDequeueFailsWhenEmpty()
    {
        var queue = new InterlockedQueue<int>();

        var result = queue.TryDequeue(out var value);

        Assert.False(result);
        Assert.Equal(default, value);
    }

    /// <summary>
    /// Enqueue an item.
    /// </summary>
    [Fact]
    [Trait("Category", "Unit")]
    public void TestEnqueue()
    {
        var queue = new InterlockedQueue<int>();

        queue.Enqueue(1);
        queue.Enqueue(2);

        Assert.Equal(2, queue.Count);
        Assert.True(queue.TryDequeue(out var value));
        Assert.Equal(1, value);
    }

    /// <summary>
    /// Clear the <see cref="InterlockedQueue{T}"/>.
    /// </summary>
    [Fact]
    [Trait("Category", "Unit")]
    public void TestClear()
    {
        var queue = new InterlockedQueue<int>([1, 2, 3]);

        queue.Clear();

        Assert.Equal(0, queue.Count);
        Assert.True(queue.Empty);
    }

    /// <summary>
    /// Enqueue and dequeue items concurrently from multiple threads without losing or corrupting data.
    /// </summary>
    [Fact]
    [Trait("Category", "Unit")]
    public void TestConcurrentEnqueueDequeue()
    {
        var queue = new InterlockedQueue<int>();
        const int itemsPerThread = 1000;
        const int threadCount = 8;

        Parallel.For(0, threadCount, i => {
            for (var j = 0; j < itemsPerThread; j++)
            {
                queue.Enqueue(i * itemsPerThread + j);
            }
        });

        Assert.Equal(itemsPerThread * threadCount, queue.Count);

        var dequeued = 0;

        while (queue.TryDequeue(out _))
        {
            dequeued++;
        }

        Assert.Equal(itemsPerThread * threadCount, dequeued);
        Assert.True(queue.Empty);
    }
}