// Copyright © Spatial Corporation. All rights reserved.

"use client";

import { useState } from "react";

import { Objectives } from "./objectives";
import { Balance } from "./balance";
import { Profits } from "./profits";
import { History } from "./history";

import { Card, Container, createElement, Icon, Link, Span, ToggleGroup } from "@sptlco/design";

export const PERIODS = {
  "24h": "24H",
  "7d": "7D",
  "30d": "30D",
  "1y": "1Y"
};

/**
 * An automated trading dashboard.
 */
export const Trades = createElement<typeof Card.Root>((props, ref) => {
  const [period, setPeriod] = useState<keyof typeof PERIODS>("24h");

  return (
    <Card.Root {...props} ref={ref}>
      <Container className="px-10">
        <Link href="/platform/logistics" className="group text-hint! flex gap-4">
          <Icon symbol="arrow_left_alt" className="transition-all duration-300 group-hover:-translate-x-1.5 group-active:-translate-x-1.5" />
          <Span>Logistics</Span>
        </Link>
      </Container>
      <Card.Header className="px-10">
        <Card.Title className="col-span-full text-5xl xl:text-9xl font-extrabold leading-snug!">Trader</Card.Title>
        <Card.Description className="xl:text-xl font-light">Automated trade analysis and execution.</Card.Description>
      </Card.Header>
      <Card.Content className="flex flex-col gap-10">
        <Container className="flex w-full">
          <Balance className="grow" period={period} />
          <Container className="flex flex-col items-end justify-between gap-10">
            <ToggleGroup.Root
              className="rounded-lg bg-background-subtle flex items-center overflow-hidden"
              type="single"
              value={period}
              onValueChange={(value) => {
                if (value) setPeriod(value as keyof typeof PERIODS);
              }}
            >
              {Object.entries(PERIODS).map((period, i) => (
                <ToggleGroup.Item
                  key={i}
                  value={period[0]}
                  className="px-5 py-2 uppercase font-extrabold text-hint rounded-lg data-[state=on]:bg-button data-[state=on]:text-foreground-primary"
                >
                  {period[1]}
                </ToggleGroup.Item>
              ))}
            </ToggleGroup.Root>
            <Profits />
          </Container>
        </Container>
        <Objectives />
        <History />
      </Card.Content>
    </Card.Root>
  );
});
