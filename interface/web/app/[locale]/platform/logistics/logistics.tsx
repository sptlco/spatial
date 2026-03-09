// Copyright © Spatial Corporation. All rights reserved.

"use client";

import { Header } from "@/elements";

import { Card, createElement, Link } from "@sptlco/design";

/**
 * A dashboard for managing assets, automated trades,
 * inventory, shipping, and sales operations.
 */
export const Logistics = createElement<typeof Card.Root>((props, ref) => {
  return (
    <Card.Root {...props} ref={ref} className="relative flex flex-col grow ">
      <Header title="Logistics" />
      <Card.Content className="flex flex-col grow gap-10 xl:gap-20! xl:pr-10">
        <Link href="/platform/logistics/assets">Assets</Link>
        <Link href="/platform/logistics/shipments">Shipments</Link>
      </Card.Content>
    </Card.Root>
  );
});
