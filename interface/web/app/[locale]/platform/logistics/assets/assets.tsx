// Copyright © Spatial Corporation. All rights reserved.

"use client";

import { useState } from "react";

import { Snapshot } from "./snapshot";
import { Activity } from "./activity";
import { Balance, PERIODS } from "./balance";
import { Delta } from "./delta";

import { Card, Container, createElement } from "@sptlco/design";
import { Header } from "@/elements";

/**
 * An automated asset management dashboard.
 */
export const Assets = createElement<typeof Card.Root>((props, ref) => {
  const [period, setPeriod] = useState<keyof typeof PERIODS>("24h");

  return (
    <Card.Root {...props} ref={ref}>
      <Header title="Assets" />
      <Card.Content className="flex flex-col gap-16 xl:gap-20 xl:pr-10">
        <Container className="flex flex-col gap-16 xl:gap-20 xl:pb-10 w-full xl:flex-row">
          <Balance period={period} onPeriodChange={setPeriod} className="w-full xl:grow" />
          <Delta className="w-full xl:w-auto" />
        </Container>
        <Snapshot period={period} />
        <Activity />
      </Card.Content>
    </Card.Root>
  );
});
