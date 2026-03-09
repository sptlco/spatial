// Copyright © Spatial Corporation. All rights reserved.

"use client";

import { useState } from "react";

import { Snapshot } from "./snapshot";
import { Activity } from "./activity";
import { Balance, PERIODS } from "./balance";
import { Delta } from "./delta";

import { Container, createElement } from "@sptlco/design";
import { Application } from "@/elements";

/**
 * An automated asset management dashboard.
 */
export const Assets = createElement<typeof Application.Root>((props, ref) => {
  const [period, setPeriod] = useState<keyof typeof PERIODS>("24h");

  return (
    <Application.Root {...props} ref={ref} title="Assets">
      <Application.Content>
        <Container className="flex flex-col xl:flex-row gap-10 xl:gap-20 w-full">
          <Balance period={period} onPeriodChange={setPeriod} className="w-full xl:grow" />
          <Delta className="w-full xl:w-auto" />
        </Container>
        <Snapshot period={period} />
        <Activity />
      </Application.Content>
    </Application.Root>
  );
});
