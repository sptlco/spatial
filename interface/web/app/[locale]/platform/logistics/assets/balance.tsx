// Copyright © Spatial Corporation. All rights reserved.

"use client";

import { Spatial } from "@sptlco/client";
import { Metric } from "@sptlco/data";
import { clsx } from "clsx";
import { useEffect, useMemo, useState } from "react";
import { Area, AreaChart, Line, LineChart, ResponsiveContainer, Tooltip, XAxis, YAxis } from "recharts";
import useSWR from "swr";

import { Container, createElement, H2, Icon, Path, Portal, Span, Svg, ToggleGroup } from "@sptlco/design";

export const PERIODS = {
  "24h": "24H",
  "7d": "7D",
  "30d": "30D",
  "1y": "1Y"
};

export const Balance = createElement<typeof Container, { period: keyof typeof PERIODS; onPeriodChange: (period: keyof typeof PERIODS) => void }>(
  ({ onPeriodChange, ...props }, ref) => {
    const [point, setPoint] = useState<number | null>(null);

    useEffect(() => {
      onPeriodChange(props.period);
    }, [props.period]);

    const from = useMemo(() => {
      return getFromDate(props.period);
    }, [props.period]);

    const ethereum = useSWR(["platform/logistics/trades/balance/ethereum", props.period], () => Spatial.metrics.read("ethereum", from), {
      refreshInterval: 10000,
      dedupingInterval: 15000
    });

    const metrics = ethereum.data && !ethereum.data.error ? ethereum.data.data : undefined;
    const now = metrics && metrics[metrics.length - 1];

    const data = useMemo(() => {
      if (!metrics) {
        return [];
      }

      return metrics.map((metric: any) => ({
        date: new Date(metric.created),
        value: getValue(metric)
      }));
    }, [metrics]);

    const chartData = useMemo(() => {
      return data.map((d) => ({
        date: d.date.getTime(),
        value: d.value
      }));
    }, [data]);

    const hovered = point != null ? chartData[point] : null;

    const value = hovered?.value ?? null;
    const date = hovered?.date != null ? new Date(hovered.date) : now ? new Date(now.created) : null;

    const base = metrics && metrics.length ? getValue(metrics[0]) : 0;
    const current = value ?? (now ? getValue(now) : 0);
    const delta = base && current ? (current - base) / base : 0;

    const color = useMemo(() => {
      if (!data || data.length < 2) {
        return "var(--color-foreground-primary)";
      }

      const first = data[0].value;
      const last = data[data.length - 1].value;

      if (last > first) return "var(--color-green)";
      if (last < first) return "var(--color-red)";

      return "var(--color-foreground-primary)";
    }, [data]);

    const hover = (state: any) => {
      const index = state?.activeTooltipIndex;

      if (index == null || index >= chartData.length) {
        setPoint(null);
        return;
      }

      if (index !== point) {
        setPoint(index);
      }
    };

    return (
      <Container {...props} ref={ref} className={clsx("flex flex-col gap-10 w-screen xl:w-auto", props.className)}>
        <Container className="flex flex-col gap-10">
          <Container className="flex flex-col gap-6 px-10 xl:p-0">
            <Container className="flex flex-col sm:flex-row gap-5 xl:gap-10 items-start xl:items-center xl:justify-between">
              <H2 className="text-2xl font-extrabold">Ethereum</H2>
            </Container>
            <Span className="flex flex-col gap-2">
              <Span className="text-5xl xl:text-9xl xl:-ml-1.5 font-extrabold truncate">
                {!now ? (
                  <Container className="flex items-center h-32">
                    <Span className="bg-background-highlight rounded-full h-10 w-full sm:w-sm animate-pulse flex" />
                  </Container>
                ) : (
                  formatCurrency(value ?? getValue(now))
                )}
              </Span>
              <Span className="flex items-center -ml-2.5 xl:text-2xl">
                {delta != 0 && (
                  <Span className={clsx("truncate inline-flex items-center", delta > 0 ? "text-green" : "text-red")}>
                    {delta > 0 ? <Icon symbol="arrow_drop_up" size={40} /> : <Icon symbol="arrow_drop_down" size={40} />}
                    <Span>
                      {formatCurrency(Math.abs(current - base))} ({(Math.abs(delta) * 100).toFixed(2)}%)
                    </Span>
                  </Span>
                )}
                <Span className="text-hint pl-2">{formatDate(date, point != null, props.period)}</Span>
              </Span>
            </Span>
          </Container>
          <Container className="w-full flex flex-col gap-10">
            <ResponsiveContainer className="xl:order-999 xl:mask-r-from-80% xl:mask-l-from-80%" width="100%" height={256}>
              <AreaChart
                accessibilityLayer
                data={chartData}
                margin={{ left: 0, right: 0 }}
                onMouseMove={hover}
                onTouchMove={hover}
                onTouchStart={hover}
                onTouchEnd={() => setPoint(null)}
                onMouseLeave={() => setPoint(null)}
              >
                <defs>
                  <linearGradient id="gradient" x1="0" y1="0" x2="0" y2="1">
                    <stop offset="0%" stopColor={color} stopOpacity={0.7} />
                    <stop offset="100%" stopColor={color} stopOpacity={0} />
                  </linearGradient>
                </defs>

                <XAxis hide dataKey="date" type="number" scale="time" domain={["dataMin", "dataMax"]} />
                <YAxis hide domain={["dataMin - 1", "dataMax"]} />

                <Area
                  type="monotone"
                  dataKey="value"
                  stroke={color}
                  strokeWidth={2}
                  fill="url(#gradient)"
                  dot={false}
                  activeDot={false}
                  connectNulls
                />

                <Tooltip
                  content={() => null}
                  cursor={
                    point
                      ? {
                          stroke: "var(--color-line-strong)",
                          strokeWidth: 1
                        }
                      : undefined
                  }
                />
              </AreaChart>
            </ResponsiveContainer>
            <ToggleGroup.Root
              className="xl:gap-4 flex items-center justify-center xl:justify-start overflow-hidden"
              type="single"
              value={props.period}
              onValueChange={(value) => {
                if (value) onPeriodChange(value as keyof typeof PERIODS);
              }}
            >
              {Object.entries(PERIODS).map((period, i) => (
                <ToggleGroup.Item
                  key={i}
                  value={period[0]}
                  className={clsx(
                    "text-xs xl:text-sm",
                    "px-5 py-2 uppercase font-bold text-hint rounded-full xl:rounded-lg xl:bg-button data-[state=on]:bg-button-highlight-active data-[state=on]:text-foreground-primary"
                  )}
                >
                  {period[1]}
                </ToggleGroup.Item>
              ))}
            </ToggleGroup.Root>
          </Container>
        </Container>
      </Container>
    );
  }
);

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
    maximumFractionDigits: 2,
    currencySign: "accounting"
  }).format(value)} USD`;
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

const getValue = (metric: Metric) => {
  return metric.value.balance * metric.value.price;
};
