// Copyright © Spatial Corporation. All rights reserved.

"use client";

import { Spatial } from "@sptlco/client";
import useSWR from "swr";

import { Button, Card, Container, createElement, Dropdown, H1, Icon, Span } from "@sptlco/design";

/**
 * A rich, configurable view of shipment data.
 */
export const Shipments = createElement<typeof Card.Root>((props, ref) => {
  const response = useSWR("platform/logistics/shipments", Spatial.shipments.list, {
    refreshInterval: 10000,
    dedupingInterval: 15000
  });

  // ...

  return (
    <Card.Root {...props} ref={ref}>
      <Card.Content className="flex flex-col w-full px-10 xl:p-0 gap-10 xl:gap-20">
        <Container className="flex items-center justify-between">
          <H1 className="text-2xl font-extrabold">Shipments</H1>
          <Dropdown.Root>
            <Dropdown.Trigger asChild>
              <Button intent="ghost" className="xl:hidden size-10! p-0! data-[state=open]:bg-button-ghost-active">
                <Icon symbol="keyboard_arrow_down" />
              </Button>
            </Dropdown.Trigger>
            <Dropdown.Content>
              <Dropdown.Item>
                <Icon symbol="add" />
                <Span>Create</Span>
              </Dropdown.Item>
            </Dropdown.Content>
          </Dropdown.Root>
          <Button className="hidden xl:flex">
            <Icon symbol="add" />
            <Span>Create</Span>
          </Button>
        </Container>
      </Card.Content>
    </Card.Root>
  );
});
