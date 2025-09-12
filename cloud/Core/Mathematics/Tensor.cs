// Copyright © Spatial Corporation. All rights reserved.

namespace Spatial.Mathematics;

/// <summary>
/// A multi-dimensional value.
/// </summary>
public class Tensor
{
    private readonly float? _scalar;
    private readonly Tensor[]? _children;

    /// <summary>
    /// Create a new <see cref="Tensor"/>.
    /// </summary>
    /// <param name="scalar">A scalar value.</param>
    private Tensor(float scalar)
    {
        _scalar = scalar;
    }

    /// <summary>
    /// Create a new <see cref="Tensor"/>.
    /// </summary>
    /// <param name="values">The tensor's children.</param>
    private Tensor(Tensor[] values)
    {
        _children = values;
    }

    /// <summary>
    /// Get a nested <see cref="Tensor"/>.
    /// </summary>
    /// <param name="dimension">The tensor's dimension.</param>
    /// <returns></returns>
    public Tensor this[int dimension]
    {
        get => _children?.ElementAtOrDefault(dimension - 1) ?? throw new NullReference();
        set => (_children ?? throw new NullReference())[dimension - 1] = value;
    }

    /// <summary>
    /// The tensor's value.
    /// </summary>
    public float Scalar => _scalar ?? throw new NullReference();

    /// <summary>
    /// The tensor's children.
    /// </summary>
    public Tensor[] Children => _children ?? throw new NullReference();

    /// <summary>
    /// Cast the <see cref="Tensor"/> to a scalar value.
    /// </summary>
    /// <param name="tensor">A <see cref="Tensor"/>.</param>
    public static implicit operator float(Tensor tensor) => tensor.Scalar;

    /// <summary>
    /// Cast the scalar value to a <see cref="Tensor"/>.
    /// </summary>
    /// <param name="scalar">A scalar value.</param>
    public static implicit operator Tensor(float scalar) => new Tensor(scalar);

    /// <summary>
    /// Cast the <see cref="Tensor"/> to an array of tensors.
    /// </summary>
    /// <param name="tensor">Cast the <see cref="Tensor"/> to an array of tensors.</param>
    public static implicit operator Tensor[](Tensor tensor) => tensor.Children;

    /// <summary>
    /// Cast an array of tensors to a <see cref="Tensor"/>.
    /// </summary>
    /// <param name="children">An array of tensors.</param>
    public static implicit operator Tensor(Tensor[] children) => new Tensor(children);

    /// <summary>
    /// Create a new zero <see cref="Tensor"/>.
    /// </summary>
    /// <param name="shape">The shape of the <see cref="Tensor"/>.</param>
    /// <returns>A zero <see cref="Tensor"/>.</returns>
    public static Tensor Zero(int[] shape)
    {
        return Create(shape, _ => 0.0F);
    }

    /// <summary>
    /// Create a new random <see cref="Tensor"/>.
    /// </summary>
    /// <param name="shape">The shape of the <see cref="Tensor"/>.</param>
    /// <returns>A random <see cref="Tensor"/>.</returns>
    public static Tensor Random(int[] shape)
    {
        return Create(shape, _ => Strong.Float(1.0F));
    }

    /// <summary>
    /// Create a new <see cref="Tensor"/>.
    /// </summary>
    /// <param name="shape">The shape of the <see cref="Tensor"/>.</param>
    /// <param name="factory">A method to create the <see cref="Tensor"/>.</param>
    /// <returns>A <see cref="Tensor"/>.</returns>
    public static Tensor Create(int[] shape, Func<int, float> factory)
    {
        return Create(shape, 0, factory);
    }

    private static Tensor Create(int[] shape, int offset, Func<int, float> factory)
    {
        var dimension = shape.Length - offset;

        if (dimension == 0)
        {
            return new Tensor(factory(dimension));
        }

        var size = shape[offset];
        var children = new Tensor[size];

        for (var i = 0; i < size; i++)
        {
            children[i] = Create(shape, offset + 1, factory);
        }

        return new Tensor(children);
    }
}