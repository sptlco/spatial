// Copyright © Spatial Corporation. All rights reserved.

"use client";

import { clsx } from "clsx";
import { useState } from "react";

import { Container, createElement, Field, Form, H2, Icon, Pagination, ScrollArea, Table } from "@sptlco/design";

type Metric = {
  name: string;
};

/**
 * An element that displays the current trading objectives.
 */
export const Activity = createElement<typeof Container>((props, ref) => {
  const [search, setSearch] = useState("");

  const metrics: Metric[] = [
    { name: "Order" },
    { name: "Created" },
    { name: "Transaction" },
    { name: "Trade" },
    { name: "Status" },
    { name: "Direction" },
    { name: "Pair" },
    { name: "Volume" },
    { name: "Price" },
    { name: "Slippage" },
    { name: "Gas Fee" },
    { name: "Gas Price" }
  ];

  return (
    <Container {...props} ref={ref} className={clsx("flex flex-col gap-10 w-screen xl:w-auto", props.className)}>
      <H2 className="px-10 text-2xl font-bold">Activity</H2>
      <Container className="flex flex-col w-full">
        <Container className="flex px-10">
          <Form
            className="relative w-full max-w-sm flex items-center"
            onSubmit={(e) => {
              e.preventDefault();
              //commitKeywords(search);
            }}
          >
            <Field
              type="text"
              id="search"
              name="search"
              placeholder="Search activity"
              value={search}
              className="w-full pl-12 pr-12"
              onChange={(e) => setSearch(e.target.value)}
              onKeyDown={(e) => {
                if (e.key === "Escape" && search.length > 0) {
                  e.preventDefault();
                  //clearKeywords();
                }
              }}
            />

            <Icon symbol="search" className="absolute left-3" />
          </Form>
        </Container>
        <ScrollArea.Root className="w-full" fade fadeOrientation="horizontal">
          <ScrollArea.Viewport className="max-w-full">
            <Table.Root className="min-w-full table-fixed text-left border-separate border-spacing-10">
              <Table.Header className="text-foreground-quaternary">
                <Table.Row>
                  {metrics.map((metric, i) => (
                    <Table.Column key={i} className="font-semibold text-sm whitespace-nowrap">
                      {metric.name}
                    </Table.Column>
                  ))}
                </Table.Row>
              </Table.Header>
            </Table.Root>
          </ScrollArea.Viewport>
          <ScrollArea.Scrollbar orientation="horizontal" />
        </ScrollArea.Root>
        <Pagination page={1} pages={1} className="self-center mt-10" />
      </Container>
    </Container>
  );
});
