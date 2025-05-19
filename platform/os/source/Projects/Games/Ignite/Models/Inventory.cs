// Copyright © Spatial. All rights reserved.

using Ignite.Assets.Types;
using Spatial.Extensions;
using Spatial.Structures;
using System;
using System.Collections.Generic;

namespace Ignite.Models;

/// <summary>
/// A collection of <see cref="Item"/> instances.
/// </summary>
public class Inventory
{
    private readonly InventoryType _type;
    private readonly SparseArray<Item> _items;

    /// <summary>
    /// Create a new <see cref="Inventory"/>.
    /// </summary>
    /// <param name="type">The inventory's <see cref="InventoryType"/>.</param>
    /// <param name="capacity">The maximum number of items that can be stored in the <see cref="Inventory"/>.</param>
    public Inventory(InventoryType type, int capacity)
    {
        _type = type;
        _items = new SparseArray<Item>(capacity);
    }

    /// <summary>
    /// Get a reference to an <see cref="Inventory"/> slot.
    /// </summary>
    /// <param name="slot">A slot in the <see cref="Inventory"/>.</param>
    /// <returns>A reference to the <see cref="Item"/> in the slot.</returns>
    public Item? this[int slot]
    {
        get => _items[slot];
        set => _items[slot] = value;
    }

    /// <summary>
    /// The inventory's <see cref="InventoryType"/>.
    /// </summary>
    public InventoryType Type => _type;

    /// <summary>
    /// The number of items in the <see cref="Inventory"/>.
    /// </summary>
    public int Count => _items.Count;

    /// <summary>
    /// The capacity of the <see cref="Inventory"/>.
    /// </summary>
    public int Capacity => _items.Capacity;

    /// <summary>
    /// Store an <see cref="Item"/> in the <see cref="Inventory"/>.
    /// </summary>
    /// <param name="item">The <see cref="Item"/> to store.</param>
    public void Store(Item item)
    {
        var (client, _) = ItemInfo.Read(item.ItemId);

        if (client.MaxLot > 1)
        {
            for (var i = 0; item.Lot > 0 && i < _items.Capacity; i++)
            {
                var stack = _items[i];

                if (stack is not null && stack.ItemId == item.ItemId && stack.Lot < client.MaxLot)
                {
                    var lot = Math.Min(item.Lot, client.MaxLot - stack.Lot);

                    item.Lot -= lot;
                    stack.Lot += lot;

                    stack.Save();
                }
            }
        }

        if (item.Lot > 0)
        {
            item.Inventory = _type;
            item.Slot = (byte) _items.Add(item);

            item.Store();
        }
    }

    /// <summary>
    /// Convert the <see cref="Inventory"/> to an array.
    /// </summary>
    /// <returns>An array of items.</returns>
    public Item[] ToArray()
    {
        return _items.ToDenseArray();
    }

    /// <summary>
    /// Convert the <see cref="Inventory"/> to an array.
    /// </summary>
    /// <typeparam name="T">The type of elements in the target array.</typeparam>
    /// <param name="selector">A selection function.</param>
    /// <returns>An array of items.</returns>
    public T[] ToArray<T>(Func<Item, T> selector)
    {
        return _items.ToDenseArray(selector);
    }

    /// <summary>
    /// Get the inventory's <see cref="IEnumerator{Item}"/>.
    /// </summary>
    /// <returns>An <see cref="IEnumerator{Item}"/>.</returns>
    public IEnumerator<Item> GetEnumerator()
    {
        foreach (var item in _items.ToArray())
        {
            if (item is not null)
            {
                yield return item;
            }
        }
    }
}