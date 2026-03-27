// Copyright © Spatial Corporation. All rights reserved.

"use client";

import useSWR from "swr";

import { Card, createElement } from "@sptlco/design";

/**
 * Don't throw your life away.
 */
export const Shipments = createElement<typeof Card.Root>((props, ref) => {
  // ...

  return <Card.Root {...props} ref={ref} />;
});
