// Copyright © Spatial. All rights reserved.

using Ignite.Components;
using Ignite.Models.Objects;
using Serilog;
using Spatial.Simulation;
using System;


namespace Ignite.Models;

/// <summary>
/// A reference to an object in the <see cref="World"/>.
/// </summary>
public abstract class Object
{
    protected Map _map;
    protected Entity _entity;

    protected Object(Map map, Entity entity)
    {
        _map = map;
        _entity = entity;
    }

    /// <summary>
    /// The <see cref="Models.Map"/> the object is located in.
    /// </summary>
    public Map Map => _map;

    /// <summary>
    /// The object's unique identification number.
    /// </summary>
    public Entity UID => _entity;

    /// <summary>
    /// The object's <see cref="Components.Tag"/>.
    /// </summary>
    public Tag Tag => Get<Tag>();

    /// <summary>
    /// The object's <see cref="Components.Transform"/>.
    /// </summary>
    public ref Transform Transform => ref Get<Transform>();

    /// <summary>
    /// The object's <see cref="Components.Speed"/>.
    /// </summary>
    public ref Speed Speed => ref Get<Speed>();

    /// <summary>
    /// The object's <see cref="Components.Vitals"/>.
    /// </summary>
    public ref Vitals Vitals => ref Get<Vitals>();

    /// <summary>
    /// The object's core <see cref="Components.Attributes"/>.
    /// </summary>
    public ref Attributes Attributes => ref Get<Attributes>();

    /// <summary>
    /// The object's <see cref="Components.Abilities"/>.
    /// </summary>
    public ref Abilities Abilities => ref Get<Abilities>();

    /// <summary>
    /// The object's <see cref="Components.Stones"/>.
    /// </summary>
    public ref Stones Stones => ref Get<Stones>();

    /// <summary>
    /// Reference an <see cref="Object"/>.
    /// </summary>
    /// <param name="entity">The <see cref="Entity"/> to reference.</param>
    /// <returns>An <see cref="Object"/>.</returns>
    public static Object Create(Map map, Entity entity)
    {
        var tag = map.Space.Get<Tag>(entity);

        return tag.Type switch {
            ObjectType.Chunk => new ChunkRef(map, entity),
            ObjectType.Player => new PlayerRef(map, entity),
            ObjectType.NPC => new NPCRef(map, entity),
            ObjectType.Mob => new MobRef(map, entity),
            _ => throw new ArgumentException("References of this object are not supported.")
        };
    }

    /// <summary>
    /// Get whether or not the object has a component.
    /// </summary>
    /// <typeparam name="T">The type of component to query.</typeparam>
    /// <returns>Whether or not the object has the component.</returns>
    public bool Has<T>() where T : unmanaged, IComponent
    {
        return _map.Space.Has<T>(_entity);
    }

    /// <summary>
    /// Get a component of type <typeparamref name="T"/>.
    /// </summary>
    /// <typeparam name="T">The type of component to get.</typeparam>
    /// <returns>A component of type <typeparamref name="T"/>.</returns>
    public ref T Get<T>() where T : unmanaged, IComponent
    {
        return ref _map.Space.Get<T>(_entity);
    }

    /// <summary>
    /// Add a component to the object.
    /// </summary>
    /// <typeparam name="T">The type of component to add.</typeparam>
    /// <param name="component">The component to add.</param>
    public Object Add<T>(in T component = default) where T : unmanaged, IComponent
    {
        _map.Space.Add(_entity, component);

        return this;
    }

    /// <summary>
    /// Set a component of the object.
    /// </summary>
    /// <typeparam name="T">The type of component to set.</typeparam>
    /// <param name="component">The value of the component.</param>
    public Object Set<T>(in T component) where T : unmanaged, IComponent
    {
        _map.Space.Set(_entity, component);

        return this;
    }

    /// <summary>
    /// Modify one of the object's components in-place.
    /// </summary>
    /// <typeparam name="T">The type of component to modify.</typeparam>
    /// <param name="modification">A modification.</param>
    public Object Modify<T>(Modification<T> modification) where T : unmanaged, IComponent
    {
        _map.Space.Modify(_entity, modification);

        return this;
    }

    /// <summary>
    /// Remove a component from the object.
    /// </summary>
    /// <typeparam name="T">The type of component to remove.</typeparam>
    public Object Remove<T>() where T : unmanaged, IComponent
    {
        _map.Space.Remove<T>(_entity);

        return this;
    }

    /// <summary>
    /// Destroy the object.
    /// </summary>
    public void Destroy()
    {
        var tag = Tag;

        _map.Space.Destroy(_entity);
        _map.Release(tag.Type, tag.Handle);
    }

    /// <summary>
    /// Add the <see cref="Object"/> to a <see cref="Spatial.Simulation.Group"/>.
    /// </summary>
    /// <param name="group">The <see cref="Spatial.Simulation.Group"/> to add the <see cref="Object"/> to.</param>
    /// <returns>The <see cref="Object"/> for method chaining.</returns>
    public Object Group(uint group)
    {
        _map.Space.Join(_entity, group);

        return this;
    }

    /// <summary>
    /// Ungroup the <see cref="Object"/>.
    /// </summary>
    /// <returns>The <see cref="Object"/> for method chaining.</returns>
    public Object Ungroup()
    {
        _map.Space.Exile(_entity);

        return this;
    }

    /// <summary>
    /// Snap the <see cref="Object"/> to a location.
    /// </summary>
    /// <param name="x">An X-coordinate.</param>
    /// <param name="y">A Y-coordinate.</param>
    public void Snap(in float x, in float y)
    {
        Transform = Transform with { X = x, Y = y };
    }

    /// <summary>
    /// Move the <see cref="Object"/>.
    /// </summary>
    /// <param name="transform">A target <see cref="Transform"/>.</param>
    public void Move(in Transform transform, in float speed) => Move(transform.X, transform.Y, speed);

    /// <summary>
    /// Move the <see cref="Object"/>.
    /// </summary>
    /// <param name="x">An X-coordinate.</param>
    /// <param name="y">A Y-coordinate.</param>
    public void Move(in float x, in float y, in float speed)
    {
        var dx = x - Transform.X;
        var dy = y - Transform.Y;

        var distance = 1.0F / MathF.Sqrt(dx * dx + dy * dy + 1e-8F);

        var vx = dx * distance * speed;
        var vy = dy * distance * speed;

        Add(new Destination(X: x, Y: y));
        Add(new Velocity(X: vx, Y: vy));

        Log.Debug("{Object} moving from {Transform} to {Destination}, {Map}.", Tag, Transform, Get<Destination>(), _map.Name);
    }

    /// <summary>
    /// Stop the <see cref="Object"/>.
    /// </summary>
    /// <param name="x">The object's X-coordinate.</param>
    /// <param name="y">The object's Y-coordinate.</param>
    public void Stop(in float x, in float y)
    {
        Snap(x, y);

        Remove<Velocity>();
        Remove<Destination>();

        Log.Debug("{Object} stopped at {Transform}, {Map}.", Tag, Transform, _map.Name);
    }

    /// <summary>
    /// Focus on an <see cref="Object"/>.
    /// </summary>
    /// <param name="target">The <see cref="Object"/> to target.</param>
    public void Target(Tag target)
    {
        UntargetImpl();

        _map.Space.Create(new Observer(Object: UID, Target: _map.EntityAt(target)));

        Log.Debug("{Object} targeting {Target}.", Tag, target);
    }

    /// <summary>
    /// Stop focusing on an <see cref="Object"/>.
    /// </summary>
    public void Untarget()
    {
        UntargetImpl();

        Log.Debug("{Object} no longer targeting.", Tag);
    }

    /// <summary>
    /// Enter another <see cref="Object"/>.
    /// </summary>
    /// <param name="other">Another <see cref="Object"/>.</param>
    public virtual void Enter(Object other) { }

    /// <summary>
    /// Exit another <see cref="Object"/>.
    /// </summary>
    /// <param name="other">Another <see cref="Object"/>.</param>
    public virtual void Exit(Object other) { }

    /// <summary>
    /// Convert the <see cref="Object"/> to a string.
    /// </summary>
    /// <returns>A string representation of the <see cref="Object"/>.</returns>
    public override string ToString()
    {
        return Tag.ToString();
    }

    private void UntargetImpl()
    {
        _map.Space.Remove<Observer>(o => o.Object == UID);
    }
}