// Copyright © Spatial Corporation. All rights reserved.

namespace Spatial.Cloud.Data.Orders;

/// <summary>
/// Indicates the current status of an <see cref="Order"/>.
/// </summary>
public enum OrderStatus
{
    /// <summary>
    /// The <see cref="Order"/> has been received.
    /// </summary>
    Received,

    /// <summary>
    /// The <see cref="Order"/> has been paid for.
    /// </summary>
    Paid,

    /// <summary>
    /// The <see cref="Order"/> has been fulfilled.
    /// </summary>
    Fulfilled,

    /// <summary>
    /// The <see cref="Order"/> was cancelled.
    /// </summary>
    Cancelled
}