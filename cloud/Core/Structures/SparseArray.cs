// Copyright © Spatial Corporation. All rights reserved.

namespace Spatial.Structures;

/// <summary>
/// An array of reference type <typeparamref name="T"/> of which elements may be null.
/// </summary>
/// <remarks>This data structure is not thread-safe.</remarks>
/// <typeparam name="T">The type of elements in the <see cref="SparseArray{T}"/>.</typeparam>
public class SparseArray<T> where T : class
{
    private int _size;
    private int _capacity;

    private readonly T?[] _array;

    /// <summary>
    /// Create a new <see cref="SparseArray{T}"/>.
    /// </summary>
    /// <param name="capacity">The number of elements the <see cref="SparseArray{T}"/> can hold.</param>
    public SparseArray(int capacity)
    {
        _size = 0;
        _array = new T[_capacity = capacity];
    }

    /// <summary>
    /// Create a new <see cref="SparseArray{T}"/>.
    /// </summary>
    /// <param name="capacity">The number of elements the <see cref="SparseArray{T}"/> can hold.</param>
    /// <param name="collection">The initial collection of items.</param>
    public SparseArray(int capacity, T[] collection)
    {
        _size = 0;
        _array = new T[_capacity = capacity];

        Array.Copy(collection, _array, collection.Length);
    }

    /// <summary>
    /// Access an element of the <see cref="SparseArray{T}"/>.
    /// </summary>
    /// <param name="index">The index of the element to access.</param>
    /// <returns>An element of type <typeparamref name="T"/>.</returns>
    public T? this[int index]
    {
        get => ElementAtOrDefault(index);
        set {
            if (value == null)
            {
                Remove(index);
                return;
            }

            Insert(value, index);
        }
    }

    /// <summary>
    /// The number of non-null elements in the <see cref="SparseArray{T}"/>.
    /// </summary>
    public int Count => _size;

    /// <summary>
    /// The maximum number of elements the <see cref="SparseArray{T}"/> can hold.
    /// </summary>
    public int Capacity => _capacity;

    /// <summary>
    /// Get an element of the <see cref="SparseArray{T}"/>.
    /// </summary>
    /// <param name="index">The index of the element.</param>
    /// <returns>A reference of type <typeparamref name="T"/>.</returns>
    /// <exception cref="NullReferenceException">Thrown if the element is null.</exception>
    public T ElementAt(int index)
    {
        return ElementAtOrDefault(index) ?? throw new NullReferenceException("The element does not exist.");
    }

    /// <summary>
    /// Get an element of the <see cref="SparseArray{T}"/> if it exists.
    /// </summary>
    /// <param name="index">The index of the element.</param>
    /// <returns>A reference of type <typeparamref name="T"/>.</returns>
    /// <exception cref="NullReferenceException">Thrown if the element is null.</exception>
    public T? ElementAtOrDefault(int index)
    {
        return _array[index];
    }

    /// <summary>
    /// Add an element to the <see cref="SparseArray{T}"/>.
    /// </summary>
    /// <param name="element">The element to add.</param>
    /// <returns>The added element.</returns>
    public int Add(T element)
    {
        for (var i = 0; i < _capacity; i++)
        {
            if (_array[i] == null)
            {
                _size++;
                _array[i] = element;
                
                return i;
            }
        }

        throw new InvalidOperationException("The array has reached its maximum capacity.");
    }

    /// <summary>
    /// Insert an element into the <see cref="SparseArray{T}"/>.
    /// </summary>
    /// <param name="element">The element to insert.</param>
    /// <param name="index">The index of the element.</param>
    /// <returns>The inserted value.</returns>
    public T Insert(T element, int index)
    {
        if (element == null)
        {
            Remove(index);
            return element!;
        }

        if (_array[index] == null)
        {
            _size++;
        }

        return _array[index] = element;
    }

    /// <summary>
    /// Remove an element from the <see cref="SparseArray{T}"/>.
    /// </summary>
    /// <param name="index">The index of the element to remove.</param>
    public T? Remove(int index)
    {
        var element = _array[index];

        if (element == null)
        {
            return element;
        }

        _array[index] = null;
        _size--;

        return element;
    }

    /// <summary>
    /// Clear the <see cref="SparseArray{T}"/>.
    /// </summary>
    public void Clear()
    {
        for (var i = 0; i < _capacity; i++)
        {
            _array[i] = null;
        }

        _size = 0;
    }

    /// <summary>
    /// Convert the <see cref="SparseArray{T}"/> to an array.
    /// </summary>
    /// <returns>An array.</returns>
    public T?[] ToArray()
    {
        return _array;
    }

    /// <summary>
    /// Convert the <see cref="SparseArray{T}"/> to an array.
    /// </summary>
    /// <param name="selector">A function that maps <typeparamref name="T"/> to <typeparamref name="TD"/>.</param>
    /// <returns>An array.</returns>
    public TD[] ToArray<TD>(Func<T?, TD> selector)
    {
        var array = new TD[_array.Length];

        for (var i = 0; i < _array.Length; i++)
        {
            array[i++] = selector(_array[i]);
        }

        return array;
    }

    /// <summary>
    /// Convert the <see cref="SparseArray{T}"/> to a dense array.
    /// </summary>
    /// <returns>A dense array.</returns>
    public T[] ToDenseArray()
    {
        var array = new T[_size];
        var j = 0;

        for (var i = 0; i < _array.Length; i++)
        {
            var element = _array[i];

            if (element is not null)
            {
                array[j++] = element;
            }
        }

        return array;
    }

    /// <summary>
    /// Convert the <see cref="SparseArray{T}"/> to a dense array.
    /// </summary>
    /// <param name="selector">A function that maps <typeparamref name="T"/> to <typeparamref name="TD"/>.</param>
    /// <returns>A dense array.</returns>
    public TD[] ToDenseArray<TD>(Func<T, TD> selector)
    {
        var array = new TD[_size];
        var j = 0;

        for (var i = 0; i < _array.Length; i++)
        {
            var element = _array[i];

            if (element is not null)
            {
                array[j++] = selector(element);
            }
        }

        return array;
    }
}