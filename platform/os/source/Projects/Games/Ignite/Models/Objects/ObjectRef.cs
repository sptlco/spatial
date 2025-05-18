// Copyright © Spatial. All rights reserved.

using Ignite.Components;
using Ignite.Contracts;
using Ignite.Contracts.Actions;
using Ignite.Contracts.Combat;
using Serilog;
using Spatial.Simulation;
using System;

namespace Ignite.Models.Objects;

/// <summary>
/// A reference to an object in the <see cref="World"/>.
/// </summary>
public abstract class ObjectRef
{
    protected Map _map;
    protected Entity _entity;

    protected ObjectRef(Map map, Entity entity)
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
    /// Reference an <see cref="ObjectRef"/>.
    /// </summary>
    /// <param name="entity">The <see cref="Entity"/> to reference.</param>
    /// <returns>An <see cref="ObjectRef"/>.</returns>
    public static ObjectRef Create(Map map, Entity entity)
    {
        if (!map.Space.Exists(entity))
        {
            throw new InvalidOperationException("The object does not exist.");
        }

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
    public ObjectRef Add<T>(in T component = default) where T : unmanaged, IComponent
    {
        _map.Space.Add(_entity, component);

        return this;
    }

    /// <summary>
    /// Set a component of the object.
    /// </summary>
    /// <typeparam name="T">The type of component to set.</typeparam>
    /// <param name="component">The value of the component.</param>
    public ObjectRef Set<T>(in T component) where T : unmanaged, IComponent
    {
        _map.Space.Set(_entity, component);

        return this;
    }

    /// <summary>
    /// Modify one of the object's components in-place.
    /// </summary>
    /// <typeparam name="T">The type of component to modify.</typeparam>
    /// <param name="modification">A modification.</param>
    public ObjectRef Modify<T>(Modification<T> modification) where T : unmanaged, IComponent
    {
        _map.Space.Modify(_entity, modification);

        return this;
    }

    /// <summary>
    /// Remove a component from the object.
    /// </summary>
    /// <typeparam name="T">The type of component to remove.</typeparam>
    public ObjectRef Remove<T>() where T : unmanaged, IComponent
    {
        _map.Space.Remove<T>(_entity);

        return this;
    }

    /// <summary>
    /// Release the <see cref="ObjectRef"/>.
    /// </summary>
    public void Release()
    {
        var str = ToString();

        _map.Release(this);

        Log.Debug("{Object} released.", str);
    }

    /// <summary>
    /// Add the <see cref="ObjectRef"/> to a <see cref="Spatial.Simulation.Group"/>.
    /// </summary>
    /// <param name="group">The <see cref="Spatial.Simulation.Group"/> to add the <see cref="ObjectRef"/> to.</param>
    /// <returns>The <see cref="ObjectRef"/> for method chaining.</returns>
    public ObjectRef Group(uint group)
    {
        _map.Space.Join(_entity, group);

        return this;
    }

    /// <summary>
    /// Ungroup the <see cref="ObjectRef"/>.
    /// </summary>
    /// <returns>The <see cref="ObjectRef"/> for method chaining.</returns>
    public ObjectRef Ungroup()
    {
        _map.Space.Exile(_entity);

        return this;
    }

    /// <summary>
    /// Snap the <see cref="ObjectRef"/> to a location.
    /// </summary>
    /// <param name="x">An X-coordinate.</param>
    /// <param name="y">A Y-coordinate.</param>
    public void Snap(in float x, in float y)
    {
        Transform = Transform with { X = x, Y = y };
    }

    /// <summary>
    /// Move the <see cref="ObjectRef"/> at walking speed.
    /// </summary>
    /// <param name="transform">A target location.</param>
    /// <param name="future">A <see cref="Future"/>.</param>
    public void Walk(in Transform transform, Future? future = default) => Walk(transform.X, transform.Y, future);

    /// <summary>
    /// Move the <see cref="ObjectRef"/> at walking speed.
    /// </summary>
    /// <param name="x">A target X-coordinate.</param>
    /// <param name="y">A target Y-coordinate.</param>
    /// <param name="future">A <see cref="Future"/>.</param>
    public void Walk(in float x, in float y, Future? future = default)
    {
        Move(x, y, Speed.Walking, future);

        _map.BroadcastExclusive(
            exclude: [Tag.Handle],
            command: NETCOMMAND.NC_ACT_SOMEONEMOVEWALK_CMD,
            data: new PROTO_NC_ACT_SOMEONEMOVEWALK_CMD {
                handle = Tag.Handle,
                from = new SHINE_XY_TYPE {
                    x = (uint) Transform.X,
                    y = (uint) Transform.Y
                },
                to = new SHINE_XY_TYPE {
                    x = (uint) x,
                    y = (uint) y
                },
                speed = (ushort) Speed.Walking,
                moveattr = new PROTO_NC_ACT_SOMEONEMOVEWALK_CMD.Attributes()
            });
    }

    /// <summary>
    /// Move the <see cref="ObjectRef"/> at running speed.
    /// </summary>
    /// <param name="transform">A target location.</param>
    /// <param name="future">A <see cref="Future"/>.</param>
    public void Run(in Transform transform, Future? future = default) => Run(transform.X, transform.Y, future);

    /// <summary>
    /// Move the <see cref="ObjectRef"/> at running speed.
    /// </summary>
    /// <param name="x">A target X-coordinate.</param>
    /// <param name="y">A target Y-coordinate.</param>
    /// <param name="future">A <see cref="Future"/>.</param>
    public void Run(in float x, in float y, Future? future = default)
    {
        Move(x, y, Speed.Running, future);

        _map.BroadcastExclusive(
            exclude: [Tag.Handle],
            command: NETCOMMAND.NC_ACT_SOMEONEMOVERUN_CMD,
            data: new PROTO_NC_ACT_SOMEONEMOVERUN_CMD {
                handle = Tag.Handle,
                from = new SHINE_XY_TYPE {
                    x = (uint) Transform.X,
                    y = (uint) Transform.Y
                },
                to = new SHINE_XY_TYPE {
                    x = (uint) x,
                    y = (uint) y
                },
                speed = (ushort) Speed.Running,
                moveattr = new PROTO_NC_ACT_SOMEONEMOVEWALK_CMD.Attributes()
            });
    }

    /// <summary>
    /// Move the <see cref="ObjectRef"/>.
    /// </summary>
    /// <param name="transform">A target <see cref="Transform"/>.</param>
    public void Move(in Transform transform, in float speed, Future? future = default) => Move(transform.X, transform.Y, speed, future);

    /// <summary>
    /// Move the <see cref="ObjectRef"/>.
    /// </summary>
    /// <param name="x">An X-coordinate.</param>
    /// <param name="y">A Y-coordinate.</param>
    public void Move(in float x, in float y, in float speed, Future? future = default)
    {
        var dx = x - Transform.X;
        var dy = y - Transform.Y;

        var distance = 1.0F / MathF.Sqrt(dx * dx + dy * dy + 1e-8F);

        var vx = dx * distance * speed;
        var vy = dy * distance * speed;

        var destination = new Destination(X: x, Y: y);
        var velocity = new Velocity(X: vx, Y: vy);

        if (future is not null)
        {
            future.Add(UID, destination);
            future.Add(UID, velocity);
        }
        else
        {
            Add(destination);
            Add(velocity);
        }

        Log.Debug("{Object} moving from {Transform} to {Destination}, {Map}.", this, Transform, destination, _map.Name);
    }

    /// <summary>
    /// Stop the <see cref="ObjectRef"/>.
    /// </summary>
    /// <param name="transform">The object's location.</param>
    public void Stop(in Transform transform, Future? future = default) => Stop(transform.X, transform.Y, future);

    /// <summary>
    /// Stop the <see cref="ObjectRef"/>.
    /// </summary>
    /// <param name="x">The object's X-coordinate.</param>
    /// <param name="y">The object's Y-coordinate.</param>
    public void Stop(in float x, in float y, Future? future = default)
    {
        Snap(x, y);

        if (future is not null)
        {
            future.Remove<Destination>(UID);
            future.Remove<Velocity>(UID);
        }
        else
        {
            Remove<Destination>();
            Remove<Velocity>();
        }

        _map.BroadcastExclusive(
            exclude: [Tag.Handle],
            command: NETCOMMAND.NC_ACT_SOMEONESTOP_CMD,
            data: new PROTO_NC_ACT_SOMEONESTOP_CMD {
                handle = Tag.Handle,
                loc = new SHINE_XY_TYPE {
                    x = (uint) Transform.X,
                    y = (uint) Transform.Y
                },
            });

        Log.Debug("{Object} stopped at {Transform}, {Map}.", this, Transform, _map.Name);
    }

    /// <summary>
    /// Focus on an <see cref="ObjectRef"/>.
    /// </summary>
    /// <param name="target">The <see cref="ObjectRef"/> to target.</param>
    public void Target(ObjectRef target)
    {
        UntargetImpl();

        _map.Space.Create(new Observer(Object: UID, Target: target.UID));

        var data = new PROTO_NC_BAT_TARGETINFO_CMD {
            order = 0,
            targethandle = target.Tag.Handle,
            targethp = (uint) target.Vitals.Health.Current,
            targetmaxhp = (uint) target.Vitals.Health.Maximum,
            targetsp = (uint) target.Vitals.Spirit.Current,
            targetmaxsp = (uint) target.Vitals.Spirit.Maximum,
            targetlp = (uint) target.Vitals.Light.Current,
            targetmaxlp = (uint) target.Vitals.Light.Maximum,
            targetlevel = target.Vitals.Level,
            hpchangeorder = target.Vitals.Version
        };

        if (this is PlayerRef player)
        {
            World.Command(
                connection: player.Session.Map,
                command: NETCOMMAND.NC_BAT_TARGETINFO_CMD,
                data: data,
                dispose: false);
        }

        data.order = 1;

        _map.Multicast(
            command: NETCOMMAND.NC_BAT_TARGETINFO_CMD,
            data: data,
            filter: entity => _map.Space.Exists<Observer>(o => o.Object == entity && o.Target == UID));

        Log.Debug("{Object} targeting {Target}.", this, target);
    }

    /// <summary>
    /// Stop focusing on an <see cref="ObjectRef"/>.
    /// </summary>
    public void Untarget()
    {
        UntargetImpl();

        Log.Debug("{Object} no longer targeting.", this);
    }

    /// <summary>
    /// Focus on another <see cref="ObjectRef"/>.
    /// </summary>
    /// <param name="other">Another <see cref="ObjectRef"/>.</param>
    public virtual void Focus(ObjectRef other) { }

    /// <summary>
    /// Blur another <see cref="ObjectRef"/>.
    /// </summary>
    /// <param name="other">Another <see cref="ObjectRef"/>.</param>
    public virtual void Blur(ObjectRef other) { }

    /// <summary>
    /// Enter another <see cref="ObjectRef"/>.
    /// </summary>
    /// <param name="other">Another <see cref="ObjectRef"/>.</param>
    public virtual void Enter(ObjectRef other) { }

    /// <summary>
    /// Exit another <see cref="ObjectRef"/>.
    /// </summary>
    /// <param name="other">Another <see cref="ObjectRef"/>.</param>
    public virtual void Exit(ObjectRef other) { }

    /// <summary>
    /// Convert the <see cref="ObjectRef"/> to a string.
    /// </summary>
    /// <returns>A string representation of the <see cref="ObjectRef"/>.</returns>
    public override string ToString()
    {
        return Tag.ToString();
    }

    private void UntargetImpl()
    {
        _map.Space.Remove<Observer>(o => o.Object == UID);
    }
}