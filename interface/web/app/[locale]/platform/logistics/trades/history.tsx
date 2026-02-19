// Copyright © Spatial Corporation. All rights reserved.

import { clsx } from "clsx";

import { Container, createElement, H2, Pagination, Table } from "@sptlco/design";

type Metric = {
  name: string;
};

/**
 * An element that displays the current trading objectives.
 */
export const History = createElement<typeof Container>((props, ref) => {
  const metrics: Metric[] = [
    { name: "Ticket" },
    { name: "Open Time" },
    { name: "Open Price" },
    { name: "Close Time" },
    { name: "Close" },
    { name: "Side" },
    { name: "Symbol" },
    { name: "Volume" },
    { name: "Stop Loss" },
    { name: "Take Profit" },
    { name: "Net Profit" }
  ];
  return (
    <Container {...props} ref={ref} className={clsx("flex flex-col gap-6", props.className)}>
      <H2 className="px-10 text-2xl font-bold">History</H2>
      <Container className="flex flex-col w-full p-10 gap-10">
        <Table.Root className="table-fixed w-full text-left">
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
