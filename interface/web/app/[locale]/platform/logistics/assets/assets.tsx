// Copyright © Spatial Corporation. All rights reserved.

"use client";

import { useState } from "react";

import { Address } from "./address";
import { Snapshot } from "./snapshot";
import { Activity } from "./activity";
import { Balance, PERIODS } from "./balance";
import { Profits } from "./profits";

import { Card, Container, createElement } from "@sptlco/design";

/**
 * An automated asset management dashboard.
 */
export const Assets = createElement<typeof Card.Root>((props, ref) => {
  const [period, setPeriod] = useState<keyof typeof PERIODS>("24h");

  return (
    <Card.Root {...props} ref={ref}>
      <Card.Header className="px-10">
        <Card.Title className="col-span-full text-5xl xl:text-9xl font-extrabold leading-snug!">Assets</Card.Title>
        <Card.Description className="xl:text-xl font-light">Automated Ethereum analysis and trading.</Card.Description>
      </Card.Header>
      <Card.Content className="flex flex-col gap-10">
        <Address />
        <Container className="flex flex-col xl:gap-10 w-full xl:flex-row xl:items-start">
          <Balance period={period} onPeriodChange={setPeriod} className="w-full xl:grow" />
          <Profits className="w-full xl:w-auto" />
        </Container>
        <Snapshot period={period} />
        <Activity />
      </Card.Content>
    </Card.Root>
  );
});
