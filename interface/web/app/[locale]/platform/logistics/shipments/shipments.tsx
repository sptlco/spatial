// Copyright © Spatial Corporation. All rights reserved.

import { Card, createElement } from "@sptlco/design";

/**
 * Don't throw your life away.
 */
export const Shipments = createElement<typeof Card.Root>((props, ref) => {
  return <Card.Root {...props} ref={ref} />;
});
