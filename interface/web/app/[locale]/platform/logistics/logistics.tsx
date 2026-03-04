// Copyright © Spatial Corporation. All rights reserved.

"use client";

import { Header } from "@/elements";

import { Balance } from "./balance";
import { Metrics } from "./metrics";

import { Card, createElement } from "@sptlco/design";

/**
 * A dashboard for managing assets, automated trades,
 * inventory, shipping, and sales operations.
 */
export const Logistics = createElement<typeof Card.Root>((props, ref) => {
  return (
    <Card.Root {...props} ref={ref} className="relative flex flex-col grow ">
      <Header title="Logistics" />
      <Card.Content className="flex flex-col grow justify-center gap-10 xl:gap-20! pt-10">
        <Balance />
        <Metrics />
      </Card.Content>
    </Card.Root>
  );
});
