// Copyright © Spatial Corporation. All rights reserved.

"use client";

import { Spatial } from "@sptlco/client";
import { Metric } from "@sptlco/data";
import { clsx } from "clsx";
import { useMemo, useState } from "react";
import { Line, LineChart, ResponsiveContainer, Tooltip } from "recharts";
import useSWR from "swr";

import { Container, createElement, H2, Icon, Span, ToggleGroup } from "@sptlco/design";

const PERIODS = {
  "24h": "24H",
  "7d": "7D",
  "30d": "30D",
  "1y": "1Y"
};

export const Balance = createElement<typeof Container>((props, ref) => {
  const [period, setPeriod] = useState<keyof typeof PERIODS>("24h");

  const from = useMemo(() => {
    return getFromDate(period);
  }, [period]);

  const history = useSWR("platform/logistics/trades/balance/history", () => Spatial.metrics.read("ethereum", from), {
    refreshInterval: 10000,
    dedupingInterval: 15000
  });

  const metrics = history.data && !history.data.error ? history.data.data : undefined;
  const now = metrics && metrics[metrics.length - 1];
  const diff = now && metrics && metrics.length >= 2 ? (getValue(now) - getValue(metrics[0])) / getValue(metrics[0]) : 0;

  const data =
    metrics?.map((metric: any) => ({
      date: new Date(metric.created),
      value: getValue(metric)
    })) ?? [];

  const color = useMemo(() => {
    if (!data || data.length < 2) {
      return "var(--color-blue)";
    }

    const first = data[0].value;
    const last = data[data.length - 1].value;

    if (last > first) return "var(--color-green)";
    if (last < first) return "var(--color-red)";

    return "var(--color-blue)";
  }, [data]);

  return (
    <Container {...props} ref={ref} className={clsx("flex flex-col gap-10", props.className)}>
      <Container className="flex flex-col gap-10">
        <Container className="flex flex-col gap-6">
          <Container className="flex items-start">
            <H2 className="text-2xl font-extrabold grow">Balance</H2>
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
          </Container>
          <Span className="flex items-center gap-4">
            <Span className="text-7xl font-bold">{now && formatCurrency(getValue(now))}</Span>
            {diff != 0 && (
              <Span className={clsx("inline-flex items-center text-2xl", diff > 0 ? "text-green" : "text-red")}>
                {diff > 0 ? <Icon symbol="arrow_drop_up" size={40} /> : <Icon symbol="arrow_drop_down" size={40} />}
                <Span>{(diff * 100).toFixed(2)}%</Span>
              </Span>
            )}
          </Span>
        </Container>
        <ResponsiveContainer className="bg-grid mask-l-from-80% mask-r-from-80%" height={310}>
          <Container className="relative h-full w-full">
            <Container className="absolute inset-0 pointer-events-none size-full" />
            <LineChart accessibilityLayer data={data} margin={{ left: 12, right: 12 }}>
              <Line dataKey="value" type="linear" stroke={color} strokeWidth={2} dot={false} activeDot={false} />
              <Tooltip
                content={() => null}
                cursor={{
                  stroke: color,
                  strokeWidth: 2,
                  strokeDasharray: "3 6"
                }}
              />
            </LineChart>
          </Container>
        </ResponsiveContainer>
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

function getFromDate(period: keyof typeof PERIODS) {
  const now = new Date();

  switch (period) {
    case "24h":
      return new Date(now.getTime() - 24 * 60 * 60 * 1000);
    case "7d":
      return new Date(now.getTime() - 7 * 24 * 60 * 60 * 1000);
    case "30d":
      return new Date(now.getTime() - 30 * 24 * 60 * 60 * 1000);
    case "1y":
      return new Date(now.getTime() - 365 * 24 * 60 * 60 * 1000);
    default:
      return undefined;
  }
}

const getValue = (metric: Metric) => {
  return metric.value.balance * metric.value.price;
};
