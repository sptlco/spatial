// Copyright © Spatial Corporation. All rights reserved.

"use client";

import { usePlatform } from "@/utilities";
import { Balance } from "./balance";
import { Metrics } from "./metrics";
import { Ticker } from "./ticker";

import { Card, createElement } from "@sptlco/design";

/**
 * A dashboard for managing assets, automated trades,
 * inventory, shipping, and sales operations.
 */
export const Logistics = createElement<typeof Card.Root>((props, ref) => {
  const { name } = usePlatform();

  return (
    <Card.Root {...props} ref={ref} className="relative flex flex-col grow ">
      <Card.Header className="px-10">
        <Card.Title className="col-span-full text-5xl xl:text-9xl xl:-translate-x-2 font-extrabold leading-snug!">Logistics</Card.Title>
        <Card.Description className="xl:text-xl font-light">Monitor and manage {name}&apos;s operations.</Card.Description>
      </Card.Header>
      <Card.Content className="flex flex-col grow justify-center gap-10 xl:gap-20! pt-10 bg-background-subtle xl:bg-transparent">
        <Balance />
        <Metrics />
      </Card.Content>
      <Card.Footer>
        <Ticker />
      </Card.Footer>
    </Card.Root>
  );
});
