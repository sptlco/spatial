// Copyright © Spatial Corporation. All rights reserved.

"use client";

import { Spatial } from "@sptlco/client";
import { clsx } from "clsx";
import { ReactNode, useMemo } from "react";
import useSWR from "swr";

import { PERIODS } from "./balance";

import { Container, createElement, H2, Icon, Paragraph, Span, Tooltip } from "@sptlco/design";

type Metric = {
  label: string;
  tip?: ReactNode;
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

  const trades = transactions?.data || [];

  const tradeCount = trades.length;
  const volume = trades.reduce((sum, t) => sum + Math.abs(t.value.volume), 0);
  const gas = trades.reduce((sum, t) => sum + t.value.gas, 0);

  const metrics: Metric[] = [
    {
      label: "Transactions",
      value: formatNumber(tradeCount),
      tip: (
        <>
          <Paragraph>Total number of transactions executed autonomously during the selected period.</Paragraph>
          <Paragraph>
            Each trade represents a completed buy or sell decision by the system. Higher trade counts generally indicate increased strategy activity
            and responsiveness to market conditions.
          </Paragraph>
        </>
      )
    },
    {
      label: "Volume",
      value: formatCurrency(volume),
      tip: (
        <>
          <Paragraph>Aggregate notional value of Ethereum transacted during the selected period.</Paragraph>
          <Paragraph>
            This reflects the total capital deployed across all transactions and serves as a key indicator of market exposure, liquidity utilization,
            and overall strategy footprint.
          </Paragraph>
        </>
      )
    },
    {
      label: "Gas",
      value: formatCurrency(gas),
      tip: (
        <>
          <Paragraph>Total network transaction fees (gas) consumed during the selected period.</Paragraph>
          <Paragraph>
            This represents the operational cost of transactions on-chain and directly impacts net performance, as higher gas consumption reduces
            realized returns.
          </Paragraph>
        </>
      )
    }
  ];

  return (
    <Container
      {...props}
      ref={ref}
      className={clsx("xl:bg-transparent", "flex w-screen xl:w-auto px-10 xl:p-0 flex-col gap-6 xl:gap-10", props.className)}
    >
      <H2 className="text-2xl font-bold text-center xl:text-left">{formatDate(null, false, props.period)}</H2>
      <Container className="flex flex-col items-center gap-10 xl:flex-row xl:justify-between w-full">
        {metrics.map((metric, i) => (
          <Container key={i} className="grow flex flex-col items-center gap-4 whitespace-nowrap">
            <Span className="text-sm text-foreground-quaternary font-semibold flex items-center divide-y divide-line-base divide-dotted gap-2">
              <Span>{metric.label}</Span>
              {metric.tip && (
                <Tooltip.Root>
                  <Tooltip.Trigger className="flex cursor-pointer">
                    <Icon symbol="info" className="font-semibold" size={16} fill />
                  </Tooltip.Trigger>
                  <Tooltip.Content sideOffset={20} className="py-4! max-w-sm text-sm text-foreground-secondary flex items-center gap-4">
                    <Icon symbol="info" className="font-semibold text-foreground-quaternary" fill size={16} />
                    <Span className="flex flex-col gap-2">{metric.tip}</Span>
                  </Tooltip.Content>
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

function formatDate(date?: Date | null, hovering?: boolean, period?: keyof typeof PERIODS) {
  if (!hovering) {
    switch (period) {
      case "24h":
        return "Past 24 hours";
      case "7d":
        return "Past week";
      case "30d":
        return "Past 30 days";
      case "1y":
        return "Past year";
    }
  }

  if (!hovering) return "Today";
  if (!date) return "";

  const datePart = new Intl.DateTimeFormat("en-US", {
    month: "short", // e.g., Mar
    day: "numeric", // e.g., 3
    timeZone: "UTC" // force UTC
  }).format(date);

  const timePart = new Intl.DateTimeFormat("en-US", {
    hour: "2-digit",
    minute: "2-digit",
    hour12: false, // 24-hour format
    timeZone: "UTC" // force UTC
  }).format(date);

  return `${datePart} at ${timePart}`;
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
