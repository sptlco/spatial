// Copyright © Spatial Corporation. All rights reserved.

using Spatial.Cloud.Data.Orders;
using Spatial.Extensions;
using Spatial.Persistence;
using Stripe;

namespace Spatial.Cloud.API;

/// <summary>
/// A <see cref="Controller"/> for events.
/// </summary>
[Path("events")]
public class EventController : Controller
{
    /// <summary>
    /// Process an event from Stripe.
    /// </summary>
    [POST]
    [Path("stripe")]
    public async Task ProcessStripeEventAsync()
    {
        var json = await new StreamReader(HttpContext.Request.Body).ReadToEndAsync();

        try
        {
            var metadata = EventUtility.ParseEvent(json);
            var signature = Request.Headers["Stripe-Signature"];

            metadata = EventUtility.ConstructEvent(json, signature, Server.Current.Configuration.Stripe.Webhooks.Secret);

            switch (metadata.Type)
            {
                // ...
            }
        }
        catch (StripeException exception)
        {
            throw new BadRequest(exception.Message);
        }
    }
}