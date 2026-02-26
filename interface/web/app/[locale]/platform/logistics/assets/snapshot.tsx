// Copyright © Spatial Corporation. All rights reserved.

"use client";

import { clsx } from "clsx";
import { ReactNode } from "react";
import useSWR from "swr";

import { Container, createElement, H2, Icon, Span, Tooltip } from "@sptlco/design";
import { Spatial } from "@sptlco/client";

type Metric = {
  label: string;
  tip?: string;
  value: ReactNode;
};

/**
 * An element that displays the current trading objectives.
 */
export const Snapshot = createElement<typeof Container>((props, ref) => {
  const transactions = useSWR("platform/logistics/assets/objectives/transactions", async () => {
    return await Spatial.metrics.read("transaction", undefined, undefined, undefined, "1m");
  });

  const ethereum = useSWR("platform/logistics/assets/objectives/ethereum", async () => {
    return await Spatial.metrics.read("ethereum", undefined, undefined, undefined, "1m");
  });

  const dollar = useSWR("platform/logistics/assets/objectives/dollar", async () => {
    return await Spatial.metrics.read("dollar", undefined, undefined, undefined, "1m");
  });

  const trades = (transactions?.data && !transactions.data.error && transactions.data?.data) || [];
  const metrics_ethereum = (ethereum?.data && !ethereum.data.error && ethereum.data?.data) || [];
  const metrics_dollar = (dollar?.data && !dollar.data.error && dollar.data?.data) || [];

  const latest_ethereum = metrics_ethereum?.[metrics_ethereum.length - 1];
  const latest_dollar = metrics_dollar?.[metrics_dollar.length - 1];

  const tradeCount = trades.length;
  const volume = trades.reduce((sum, t) => sum + t.value.volume, 0);
  const deviation = trades.reduce((sum, t) => sum + t.value.deviation, 0) / tradeCount || 0;
  const slippage = trades.reduce((sum, t) => sum + t.value.slippage, 0) / tradeCount || 0;
  const gas = trades.reduce((sum, t) => sum + t.value.gas, 0) / tradeCount || 0;

  const balance_ethereum = latest_ethereum?.value.balance ?? 0;
  const price_ethereum = latest_ethereum?.value.price ?? 0;
  const balance_dollar = latest_dollar?.value.balance ?? 0;
  const value = balance_ethereum * price_ethereum + balance_dollar;

  const metrics: Metric[] = [
    { label: "Trades", value: formatNumber(tradeCount) },
    { label: "Volume", value: formatCurrency(volume) },
    { label: "Average Deviation", value: formatPercent(deviation), tip: "Average price deviation at trade entry." },
    { label: "Average Slippage", value: formatPercent(slippage) },
    { label: "Average Gas", value: formatCurrency(gas) },
    { label: "Portfolio", value: formatCurrency(value) }
  ];

  return (
    <Container {...props} ref={ref} className={clsx("flex w-screen xl:w-auto flex-col gap-6", props.className)}>
      <H2 className="px-10 text-2xl font-bold">Snapshot</H2>
      <Container className="flex flex-col xl:flex-row w-full xl:rounded-4xl xl:bg-background-surface">
        {metrics.map((metric, i) => (
          <Container key={i} className="grow flex flex-col gap-4 p-10 whitespace-nowrap">
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
