// Copyright © Spatial Corporation. All rights reserved.

import { Balance } from "./balance";
import { Metrics } from "./metrics";
import { Ticker } from "./ticker";

import { Card, Container, createElement } from "@sptlco/design";

/**
 * A dashboard for managing assets, automated trades,
 * inventory, shipping, and sales operations.
 */
export const Logistics = createElement<typeof Card.Root>((props, ref) => {
  return (
    <Card.Root {...props} ref={ref} className="flex flex-col gap-0! xl:gap-10 pb-10">
      <Ticker />
      <Card.Header className="px-10 pt-10">
        <Card.Title className="col-span-full">
          <Balance />
        </Card.Title>
      </Card.Header>
      <Container className="flex flex-col gap-10 mt-10!">
        <Metrics />
      </Container>
    </Card.Root>
  );
});
