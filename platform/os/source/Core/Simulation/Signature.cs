// Copyright © Spatial Corporation. All rights reserved.

using System.Numerics;

namespace Spatial.Simulation;

/// <summary>
/// A composition of bits that specify the component structure 
/// of an <see cref="Entity"/>.
/// </summary>
/// <param name="Id">An identification number.</param>
public partial record struct Signature(UInt128 Id)
{
    private List<Type>? _components;

    /// <summary>
    /// An empty <see cref="Signature"/>.
    /// </summary>
    public static readonly Signature Empty = new();

    /// <summary>
    /// The number of components in the <see cref="Signature"/>.
    /// </summary>
    public readonly int Count = BitOperations.PopCount((ulong) (Id >> 64)) + BitOperations.PopCount((ulong) Id);
    
    /// <summary>
    /// The component types of the <see cref="Signature"/>.
    /// </summary>
    public List<Type> Components => _components ??= [.. GetComponentTypes()]; 

    /// <summary>
    /// Compute the bitwise-or of two signatures.
    /// </summary>
    /// <param name="left">The <see cref="Signature"/> on the left side of the operation.</param>
    /// <param name="right">The <see cref="Signature"/> on the right side of the operation.</param>
    /// <returns>The bitwise-or of <paramref name="left"/> and <paramref name="right"/>.</returns>
    public static Signature operator |(in Signature left, in Signature right)
    {
        return new(left.Id | right.Id);
    }

    /// <summary>
    /// Compute the bitwise-and of two signatures.
    /// </summary>
    /// <param name="left">The <see cref="Signature"/> on the left side of the operation.</param>
    /// <param name="right">The <see cref="Signature"/> on the right side of the operation.</param>
    /// <returns>The bitwise-and of <paramref name="left"/> and <paramref name="right"/>.</returns>
    public static Signature operator &(in Signature left, in Signature right)
    {
        return new(left.Id & right.Id);
    }

    /// <summary>
    /// Compute the ones-complement of a <see cref="Signature"/>.
    /// </summary>
    /// <param name="signature">A <see cref="Signature"/>.</param>
    /// <returns>The ones-complement of <paramref name="signature"/>.</returns>
    public static Signature operator ~(in Signature signature)
    {
        return new(~signature.Id);
    }

    /// <summary>
    /// Determine whether or not a <see cref="Signature"/> equals another <see cref="Signature"/>.
    /// </summary>
    /// <param name="left">A <see cref="Signature"/>.</param>
    /// <param name="right">Another <see cref="Signature"/>.</param>
    /// <returns>True if <paramref name="left"/> equals <paramref name="right"/>; otherwise false.</returns>
    public static bool operator ==(in Signature left, in Signature right)
    {
        return left.Equals(right);
    }

    /// <summary>
    /// Determine whether or not a <see cref="Signature"/> does not equal another <see cref="Signature"/>.
    /// </summary>
    /// <param name="left">A <see cref="Signature"/>.</param>
    /// <param name="right">Another <see cref="Signature"/>.</param>
    /// <returns>True if <paramref name="left"/> does not equal <paramref name="right"/>; otherwise false.</returns>
    public static bool operator !=(in Signature left, in Signature right)
    {
        return !left.Equals(right);
    }

    /// <summary>
    /// Get whether or not this <see cref="Signature"/> contains all of the 
    /// bits of another <see cref="Signature"/>.
    /// </summary>
    /// <param name="signature">A <see cref="Signature"/>.</param>
    /// <returns>The result of the comparison.</returns>
    public readonly bool All(in Signature signature)
    {
        return (Id & signature.Id) == signature.Id;
    }

    /// <summary>
    /// Get whether or not this <see cref="Signature"/> contains any of the 
    /// bits of another <see cref="Signature"/>.
    /// </summary>
    /// <param name="signature">A <see cref="Signature"/>.</param>
    /// <returns>The result of the comparison.</returns>
    public readonly bool Any(in Signature signature)
    {
        return (Id & signature.Id) != 0;
    }

    /// <summary>
    /// Get whether or not this <see cref="Signature"/> contains none of the 
    /// bits of another <see cref="Signature"/>.
    /// </summary>
    /// <param name="signature">A <see cref="Signature"/>.</param>
    /// <returns>The result of the comparison.</returns>
    public readonly bool None(in Signature signature)
    {
        return (Id & signature.Id) == 0;
    }

    /// <summary>
    /// Compute the <see cref="Signature"/> of a component <see cref="Type"/>.
    /// </summary>
    /// <typeparam name="T">A component <see cref="Type"/>.</typeparam>
    /// <returns>A <see cref="Signature"/>.</returns>
    public static Signature Of<T>() where T : unmanaged, IComponent
    {
        return new((UInt128) 1 << Component<T>.Id);
    }

    /// <summary>
    /// Combine multiple signatures into one <see cref="Signature"/>.
    /// </summary>
    /// <param name="signatures">An array of signatures.</param>
    /// <returns>A combined <see cref="Signature"/>.</returns>
    public static Signature Combine(params Signature[] signatures)
    {
        var signature = Empty;

        for (var i = 0; i < signatures.Length; i++)
        {
            signature |= signatures[i];
        }

        return signature;
    }

    /// <summary>
    /// Determine whether or not the <see cref="Signature"/> contains a component.
    /// </summary>
    /// <param name="type">A component <see cref="Type"/>.</param>
    /// <returns>True if the <see cref="Signature"/> contains the component; otherwise false.</returns>
    public readonly bool Includes(Type type)
    {
        return (Id & ((UInt128) 1 << ComponentRegistry.GetComponentId(type))) != 0;
    }

    /// <summary>
    /// Determine whether or not the <see cref="Signature"/> contains a component.
    /// </summary>
    /// <typeparam name="T">A component <see cref="Type"/>.</typeparam>
    /// <returns>True if the <see cref="Signature"/> contains the component; otherwise false.</returns>
    public readonly bool Includes<T>() where T : unmanaged, IComponent
    {
        return (Id & ((UInt128) 1 << Component<T>.Id)) != 0;
    }

    /// <summary>
    /// Determine whether or not the <see cref="Signature"/> equals another <see cref="Signature"/>.
    /// </summary>
    /// <param name="other">Another <see cref="Signature"/>.</param>
    /// <returns>True if the <see cref="Signature"/> equals <paramref name="other"/>; otherwise false.</returns>
    public readonly bool Equals(Signature other)
    {
        return Id == other.Id;
    }

    /// <summary>
    /// Get the hash code of the <see cref="Signature"/>.
    /// </summary>
    public readonly override int GetHashCode()
    {
        return Id.GetHashCode();
    }

    private readonly IEnumerable<Type> GetComponentTypes()
    {
        var low = (ulong) Id;
        var high = (ulong) (Id >> 64);

        while (low != 0)
        {
            int bit = BitOperations.TrailingZeroCount(low);
            yield return ComponentRegistry.GetComponentType(bit);
            low ^= 1UL << bit;
        }

        while (high != 0)
        {
            int bit = BitOperations.TrailingZeroCount(high);
            yield return ComponentRegistry.GetComponentType(bit + 64);
            high ^= 1UL << bit;
        }
    }
}