// Copyright © Spatial Corporation. All rights reserved.

import { Address } from "./address";
import { Objectives } from "./objectives";
import { Activity } from "./activity";
import { Balance } from "./balance";
import { Profits } from "./profits";

import { Card, Container, createElement, Icon, Link, Span } from "@sptlco/design";

/**
 * An automated trading dashboard.
 */
export const Trades = createElement<typeof Card.Root>((props, ref) => {
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
        <Address />
        <Container className="flex w-full gap-10">
          <Balance className="grow" />
          <Profits />
        </Container>
        <Objectives />
        <Activity />
      </Card.Content>
    </Card.Root>
  );
});
