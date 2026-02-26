// Copyright © Spatial Corporation. All rights reserved.

"use client";

import { Spatial } from "@sptlco/client";
import { clsx } from "clsx";
import { ReactNode, useMemo } from "react";
import useSWR from "swr";

import { PERIODS } from "./balance";

import { Container, createElement, H2, Icon, Span, Tooltip } from "@sptlco/design";

type Metric = {
  label: string;
  tip?: string;
  value: ReactNode;
};

/**
 * An element that displays the current trading objectives.
 */
export const Snapshot = createElement<typeof Container, { period: keyof typeof PERIODS }>((props, ref) => {
  const from = useMemo(() => {
    return getFromDate(props.period);
  }, [props.period]);

  const transactions = useSWR(["platform/logistics/assets/snapshot/transactions", props.period], async () => {
    return await Spatial.metrics.read("transaction", from, undefined, undefined, "1m");
  });

  const trades = (transactions?.data && !transactions.data.error && transactions.data?.data) || [];

  const tradeCount = trades.length;
  const volume = trades.reduce((sum, t) => sum + t.value.volume, 0);
  const gas = trades.reduce((sum, t) => sum + t.value.gas, 0);

  const metrics: Metric[] = [
    { label: "Trades", value: formatNumber(tradeCount) },
    { label: "Total Volume", value: formatCurrency(volume) },
    { label: "Total Gas", value: formatCurrency(gas) }
  ];

  return (
    <Container {...props} ref={ref} className={clsx("flex w-screen xl:w-auto flex-col gap-6", props.className)}>
      <H2 className="px-10 text-2xl font-bold">Snapshot</H2>
      <Container className="flex flex-col xl:flex-row xl:justify-between w-full xl:rounded-4xl xl:bg-background-surface">
        {metrics.map((metric, i) => (
          <Container key={i} className="grow flex flex-col xl:items-center gap-4 p-10 whitespace-nowrap">
            <Span className="text-sm text-foreground-quaternary font-semibold flex items-center gap-2">
              <Span>{metric.label}</Span>
              {metric.tip && (
                <Tooltip.Root>
                  <Tooltip.Trigger className="flex cursor-pointer">
                    <Icon symbol="info" className="font-semibold" size={20} fill />
                  </Tooltip.Trigger>
                  <Tooltip.Content sideOffset={20}>{metric.tip}</Tooltip.Content>
                </Tooltip.Root>
              )}
            </Span>
            <Span className="text-4xl flex items-center gap-4 whitespace-nowrap">
              <Span>{metric.value}</Span>
            </Span>
          </Container>
        ))}
      </Container>
    </Container>
  );
});

function formatCurrency(value?: number) {
  if (value == null || isNaN(value)) {
    return "-";
  }

  const abs = Math.abs(value);
  const format = (num: number, suffix = "") => {
    const formatted = new Intl.NumberFormat("en-US", {
      minimumFractionDigits: 0,
      maximumFractionDigits: 2
    }).format(num);

    return `${value < 0 ? "-" : ""}${formatted}${suffix} USD`;
  };

  if (abs >= 1_000_000_000_000) {
    return format(abs / 1_000_000_000_000, "T");
  }

  if (abs >= 1_000_000_000) {
    return format(abs / 1_000_000_000, "B");
  }

  if (abs >= 1_000_000) {
    return format(abs / 1_000_000, "M");
  }

  if (abs >= 1_000) {
    return format(abs / 1_000, "K");
  }

  return `${new Intl.NumberFormat("en-US", {
    minimumFractionDigits: 0,
    maximumFractionDigits: 2
  }).format(value)} USD`;
}

function formatNumber(value?: number) {
  if (value == null) return "-";
  return new Intl.NumberFormat("en-US").format(value);
}

function formatPercent(value: number) {
  return `${(value * 100).toFixed(2)}%`;
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
