// Copyright © Spatial Corporation. All rights reserved.

"use client";

import { Application } from "@/elements";

import { Button, Container, createElement, Icon, LI, Pagination, Separator, Span, UL } from "@sptlco/design";

type Address = {
  name?: string;
  company?: string;
  line1: string;
  line2?: string;
  city: string;
  state: string;
  zip: string;
  country: string;
};

type Shipment = {
  id: string;
  from: Address;
  to: Address;
};

export const Shipments = createElement<typeof Application.Root>((props, ref) => {
  const shipments: Shipment[] = [
    {
      id: "SHP-1055",
      from: {
        company: "Spatial Corporation",
        line1: "240 2ND AVE S",
        line2: "STE 201K",
        city: "Seattle",
        state: "WA",
        zip: "98104",
        country: "USA"
      },
      to: {
        name: "Dakarai Cundiff",
        company: "Spatial Corporation",
        line1: "2014 FAIRVIEW AVE",
        line2: "APT 2211",
        city: "Seattle",
        state: "WA",
        zip: "98121",
        country: "USA"
      }
    }
  ];

  const render = (title: string, address: Address) => {
    return (
      <Container className="flex flex-col p-10 gap-5">
        <Span className="flex items-center gap-2.5 text-2xl font-bold">
          <Span>{title}</Span>
          <Span className="text-foreground-quaternary font-normal">
            {address.city}, {address.state}
          </Span>
        </Span>
        <UL className="grid grid-cols-3 gap-10 w-full">
          {address.name && (
            <LI className="flex flex-col">
              <Span className="text-sm mb-2 text-foreground-quaternary">Name</Span>
              <Span className="truncate">{address.name}</Span>
            </LI>
          )}
          {address.company && (
            <LI className="flex flex-col">
              <Span className="text-sm mb-2 text-foreground-quaternary">Company</Span>
              <Span className="truncate">{address.company}</Span>
            </LI>
          )}
          <LI className="flex flex-col col-start-3">
            <Span className="text-sm mb-2 text-foreground-quaternary">Address</Span>
            <Span className="truncate">{address.line1}</Span>
            {address.line2 && <Span className="truncate">{address.line2}</Span>}
            <Span className="truncate">
              {address.city}, {address.state} {address.zip}
            </Span>
          </LI>
        </UL>
      </Container>
    );
  };

  return (
    <Application.Root {...props} ref={ref} title="Shipments">
      <Application.Content className="px-10 xl:p-0">
        <UL className="grid grid-cols-3 gap-10">
          {shipments.map((shipment, i) => {
            return [...Array(5)].map((_, j) => (
              <LI key={`${i}+${j}`} className="cursor-pointer flex flex-col bg-input outline outline-line-faint rounded-4xl">
                <Container className="flex flex-col p-10 gap-2">
                  <Container className="flex items-center justify-between">
                    <Span className="text-4xl font-bold">{shipment.id}</Span>
                    <Button intent="ghost" className="p-0! size-10!">
                      <Icon symbol="more_horiz" className="font-light" />
                    </Button>
                  </Container>
                  <Span className="text-foreground-quaternary text-xs uppercase">2 Parcels • 2 Units</Span>
                </Container>
                <Separator className="flex w-full h-px bg-line-faint" />
                {render("Origin", shipment.from)}
                <Separator className="flex w-full h-px bg-line-faint" />
                {render("Destination", shipment.to)}
              </LI>
            ));
          })}
        </UL>
        <Pagination pages={1} page={1} className="self-center" />
      </Application.Content>
    </Application.Root>
  );
});
