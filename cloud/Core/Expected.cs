// Copyright © Spatial Corporation. All rights reserved.

namespace Spatial;

/// <summary>
/// A value whose retrieval is error-prone.
/// </summary>
/// <typeparam name="T"></typeparam>
public class Expected<T>
{
    private readonly T? _value;
    private readonly Error? _error;

    /// <summary>
    /// Create an <see cref="Expected{T}"/>.
    /// </summary>
    /// <param name="value">The expected value.</param>
    public Expected(T? value)
    {
        _value = value;
    }

    /// <summary>
    /// Create an <see cref="Expected{T}"/>.
    /// </summary>
    /// <param name="error">An unexpected <see cref="Spatial.Error"/>.</param>
    public Expected(Error error)
    {
        _error = error;
    }

    /// <summary>
    /// The expected value.
    /// </summary>
    public T Value => _value ?? throw new NullReference();

    /// <summary>
    /// An unexpected <see cref="Spatial.Error"/>.
    /// </summary>
    public Error Error => _error ?? throw new NullReference();

    /// <summary>
    /// Convert a value of type <typeparamref name="T"/> to an <see cref="Expected{T}"/>.
    /// </summary>
    /// <param name="value">A value of type <typeparamref name="T"/>.</param>
    public static implicit operator Expected<T>(T value) => new(value);

    /// <summary>
    /// Convert an <see cref="Expected{T}"/> to a value of type <typeparamref name="T"/>.
    /// </summary>
    /// <param name="value">The expected value.</param>
    public static implicit operator T(Expected<T> value) => value.Value;

    /// <summary>
    /// Convert an <see cref="Spatial.Error"/> to an <see cref="Expected{T}"/>.
    /// </summary>
    /// <param name="error"></param>
    public static implicit operator Expected<T>(Error error) => new(error);

    /// <summary>
    /// Convert an <see cref="Expected{T}"/> to a boolean.
    /// </summary>
    /// <param name="value">The <see cref="Expected{T}"/>.</param>
    public static implicit operator bool(Expected<T> value) => value._error is null;
}