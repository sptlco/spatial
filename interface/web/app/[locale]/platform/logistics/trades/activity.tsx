// Copyright © Spatial Corporation. All rights reserved.

"use client";

import { clsx } from "clsx";
import { useState } from "react";

import { Container, createElement, Field, Form, H2, Icon, Pagination, Table } from "@sptlco/design";

type Metric = {
  name: string;
};

/**
 * An element that displays the current trading objectives.
 */
export const Activity = createElement<typeof Container>((props, ref) => {
  const [search, setSearch] = useState("");

  const metrics: Metric[] = [
    { name: "Order ID" },
    { name: "Created" },
    { name: "Transaction" },
    { name: "Trade ID" },
    { name: "Status" },
    { name: "Direction" },
    { name: "Pair" },
    { name: "Amount" },
    { name: "Price" },
    { name: "Slippage" },
    { name: "Gas Fee" },
    { name: "Gas Price" },
    { name: "Error" }
  ];

  return (
    <Container {...props} ref={ref} className={clsx("flex flex-col gap-10", props.className)}>
      <H2 className="px-10 text-2xl font-bold">Activity</H2>
      <Container className="flex flex-col w-full px-10">
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
        <Table.Root className="table-fixed w-full text-left border-separate border-spacing-y-10">
          <Table.Header className="text-foreground-quaternary">
            <Table.Row>
              {metrics.map((metric, i) => (
                <Table.Column key={i} className="font-semibold text-sm">
                  {metric.name}
                </Table.Column>
              ))}
            </Table.Row>
          </Table.Header>
        </Table.Root>
        <Pagination page={1} pages={1} className="self-center" />
      </Container>
    </Container>
  );
});
