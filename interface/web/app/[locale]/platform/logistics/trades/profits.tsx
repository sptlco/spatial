// Copyright © Spatial Corporation. All rights reserved.

"use client";

import { clsx } from "clsx";

import { Container, createElement, H2, Span, Tooltip } from "@sptlco/design";

export const Profits = createElement<typeof Container>((props, ref) => {
  const data = Array.from({ length: 90 }).map((_, i) => ({ id: i, value: (Math.random() - 0.5) * 500 }));
  const max = Math.max(...data.map((d) => Math.abs(d.value)));

  function getColor(value: number) {
    if (value === 0) {
      return "bg-background-subtle";
    }

    return value > 0 ? "bg-green" : "bg-red";
  }

  return (
    <Container {...props} ref={ref} className={clsx("flex flex-col p-10 gap-8 rounded-4xl bg-background-surface", props.className)}>
      <Container className="flex flex-col gap-6">
        <H2 className="text-2xl font-bold">Profit / Loss</H2>
        <Span className="text-5xl font-bold">{formatCurrency(7216.47)}</Span>
      </Container>
      <Container className="grid grid-flow-col auto-cols-[32px] grid-rows-7 gap-1.5 overflow-hidden">
        {data.map((day) => (
          <Tooltip.Root key={day.id} delayDuration={0}>
            <Tooltip.Trigger>
              <Container
                className={clsx("cursor-pointer w-8 h-8 rounded-lg transition-colors duration-200", getColor(day.value))}
                style={{ opacity: 0.2 + Math.min(Math.abs(day.value) / max, 1) * 0.8 }}
              />
            </Tooltip.Trigger>
            <Tooltip.Content className="flex items-center gap-4">
              <Span className="text-hint">{day.id}</Span>
              <Span>{formatCurrency(day.value)}</Span>
            </Tooltip.Content>
          </Tooltip.Root>
        ))}
      </Container>
    </Container>
  );
});

function formatCurrency(value?: number) {
  if (value == null) {
    return "-";
  }

  return new Intl.NumberFormat("en-US", {
    style: "currency",
    currency: "USD",
    currencyDisplay: "code",
    currencySign: "accounting",
    maximumFractionDigits: 2
  }).format(value);
}
