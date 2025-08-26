// Copyright © Spatial Corporation. All rights reserved.

using System.Collections.Concurrent;
using System.Reflection;

namespace Spatial.Simulation;

/// <summary>
/// A set of properties associated with an <see cref="Entity"/>.
/// </summary>
public interface IComponent
{
}

/// <summary>
/// Metadata for a <see cref="IComponent"/>.
/// </summary>
internal static unsafe class Component<T> where T : unmanaged, IComponent
{
    /// <summary>
    /// The component's identification number.
    /// </summary>
    public static readonly int Id;

    /// <summary>
    /// The component's size in bytes.
    /// </summary>
    public static readonly int Size;
    
    static Component()
    {
        Id = ComponentRegistry.GetComponentId<T>();
        Size = sizeof(T);
    }
}

/// <summary>
/// A registry of component types.
/// </summary>
internal static class ComponentRegistry
{
    private static readonly ConcurrentDictionary<int, Type> _types;
    private static readonly ConcurrentDictionary<Type, int> _bits;
    private static int _next;

    static ComponentRegistry()
    {
        _types = [];
        _bits = [];
        _next = -1;

        Register(AppDomain.CurrentDomain.GetAssemblies());

        AppDomain.CurrentDomain.AssemblyLoad += (s, e) => {
            Register(e.LoadedAssembly);
        };
    }

    /// <summary>
    /// Get the component identifier for a given type.
    /// </summary>
    /// <typeparam name="T">The component type.</typeparam>
    /// <returns>The component identifier.</returns>
    public static int GetComponentId<T>()
    {
        return GetComponentId(typeof(T));
    }

    /// <summary>
    /// Get the component identifier for a given <see cref="Type"/>.
    /// </summary>
    /// <param name="type">The component <see cref="Type"/>.</param>
    /// <returns>The component identifier.</returns>
    public static int GetComponentId(Type type)
    {
        if (!_bits.TryGetValue(type, out var bit))
        {
            bit = Register(type);
        }

        return bit;
    }

    /// <summary>
    /// Get the component type for a given identifier.
    /// </summary>
    /// <param name="bit">The component type identifier.</param>
    /// <returns>The component type.</returns>
    public static Type GetComponentType(int bit)
    {
        return _types[bit];
    }

    private static void Register(params Assembly[] assemblies)
    {
        foreach (var assembly in assemblies)
        {
            assembly
                .GetTypes()
                .Where(type => type.GetInterfaces().Contains(typeof(IComponent)))
                .ToList()
                .ForEach(type => Register(type));
        }
    }

    private static int Register(Type type)
    {
        var id = Interlocked.Increment(ref _next);

        if (id >= Constants.MaxComponents)
        {
            throw new InvalidOperationException("Exceeded the maximum number of components.");
        }

        _types[_bits[type] = id] = type;

        return id;
    }
}