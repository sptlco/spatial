// Copyright © Spatial Corporation. All rights reserved.

using Spatial.Persistence;

namespace Spatial.Cloud.Data.Orders;

/// <summary>
/// An order containing one or more line items.
/// </summary>
[Collection("orders")]
public class Order : Resource
{
    /// <summary>
    /// The current status of the <see cref="Order"/>.
    /// </summary>
    public OrderStatus Status { get; set; } = OrderStatus.Received;

   // ...
}