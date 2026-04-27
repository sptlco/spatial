// Copyright © Spatial Corporation. All rights reserved.

"use client";

import { useState } from "react";

import { Snapshot } from "./snapshot";
import { Activity } from "./activity";
import { Balance, PERIODS } from "./balance";
import { Delta } from "./delta";

import { Card, Container, createElement } from "@sptlco/design";

/**
 * An automated asset management dashboard.
 */
export const Wallet = createElement<typeof Card.Root>((props, ref) => {
  const [period, setPeriod] = useState<keyof typeof PERIODS>("24h");

  return (
    <Card.Root {...props} ref={ref}>
      <Card.Content className="flex flex-col w-full gap-10 xl:gap-20">
        <Container className="flex flex-col xl:flex-row gap-10 xl:gap-20 w-full">
          <Balance period={period} onPeriodChange={setPeriod} className="w-full xl:grow" />
          <Delta className="w-full xl:w-auto" />
        </Container>
        <Snapshot period={period} />
        <Activity />
      </Card.Content>
    </Card.Root>
  );
});
