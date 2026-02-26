// Copyright © Spatial Corporation. All rights reserved.

"use client";

import { Spatial } from "@sptlco/client";
import { Metric } from "@sptlco/data";
import { clsx } from "clsx";
import { useMemo, useState } from "react";
import { Line, LineChart, ResponsiveContainer, Tooltip, XAxis, YAxis } from "recharts";
import useSWR from "swr";

import { Container, createElement, H2, Icon, Path, Portal, Span, Svg, ToggleGroup } from "@sptlco/design";

const PERIODS = {
  "24h": "24H",
  "7d": "7D",
  "30d": "30D",
  "1y": "1Y"
};

export const Balance = createElement<typeof Container>((props, ref) => {
  const [period, setPeriod] = useState<keyof typeof PERIODS>("24h");
  const [point, setPoint] = useState<number | null>(null);

  const from = useMemo(() => {
    return getFromDate(period);
  }, [period]);

  const ethereum = useSWR(["platform/logistics/trades/balance/ethereum", period], () => Spatial.metrics.read("ethereum", from), {
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

  const forecastData = useMemo(() => {
    if (data.length < 2) {
      return [];
    }

    const last = data[data.length - 1];
    const recent = data.slice(-Math.min(60, data.length));
    const slope = recent.length > 1 ? (recent[recent.length - 1].value - recent[0].value) / (recent.length - 1) : 0;
    const interval = data[data.length - 1].date.getTime() - data[data.length - 2].date.getTime();

    const horizon =
      {
        "24h": 180, // 3 hours (180 mins)
        "7d": 1440, // 24 hours (1440 mins)
        "30d": 7, // 7 days
        "1y": 90 // 90 days
      }[period] ?? 60;

    return Array.from({ length: horizon }).map((_, i) => {
      const next = new Date(last.date.getTime() + interval * (i + 1));

      return {
        date: next.getTime(),
        historyValue: null,
        forecastValue: last.value + slope * (i + 1)
      };
    });
  }, [data, period]);

  const combinedData = useMemo(() => {
    if (!data.length) {
      return [];
    }

    const history = data.map((d) => ({
      date: d.date.getTime(),
      historyValue: d.value,
      forecastValue: null
    }));

    const last = history[history.length - 1];
    const connector =
      forecastData.length > 0
        ? {
            date: last.date,
            historyValue: last.historyValue,
            forecastValue: last.historyValue
          }
        : null;

    return connector ? [...history, connector, ...forecastData] : history;
  }, [data, forecastData]);

  const base = metrics && metrics.length ? getValue(metrics[0]) : 0;
  const hovered = point != null ? combinedData[point] : null;
  const hoveredValue = hovered?.historyValue ?? hovered?.forecastValue ?? null;
  const currentValue = hoveredValue ?? (now ? getValue(now) : 0);
  const displayDate = hovered?.date != null ? new Date(hovered.date) : now ? new Date(now.created) : null;
  const isForecastHover = hovered?.forecastValue != null && hovered?.historyValue == null;
  const diff = base && currentValue ? (currentValue - base) / base : 0;

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

  return (
    <Container {...props} ref={ref} className={clsx("flex flex-col gap-10 w-screen xl:w-auto", props.className)}>
      {metrics && (
        <Portal container={document.getElementById("title")!}>
          <Container className="flex items-center gap-2.5 md:gap-4 font-extrabold truncate text-sm md:text-base">
            <Ethereum className="h-5 md:h-6" />
            <Span className="truncate">{formatCurrency(metrics[metrics.length - 1].value.price)}</Span>
          </Container>
        </Portal>
      )}
      <Container className="flex flex-col gap-10 pb-10 xl:p-10 xl:rounded-[56px]">
        <Container className="flex flex-col gap-6 px-10 xl:p-0">
          <Container className="flex flex-col sm:flex-row gap-5 xl:gap-10 items-start xl:items-center">
            <Container className="flex items-center justify-start gap-4">
              <H2 className="inline-flex text-2xl font-extrabold">Balance</H2>
              <Span className={clsx("inline-flex items-center gap-1 text-sm text-hint", !hovered && "text-yellow")}>
                <Icon symbol={point ? (isForecastHover ? "online_prediction" : "history") : "bolt"} className="font-light" size={20} fill />
                <Span className="font-bold">{formatDate(displayDate, period, hovered != null)}</Span>
              </Span>
            </Container>

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
                  className="px-5 py-2 uppercase font-bold text-hint rounded-lg data-[state=on]:bg-button data-[state=on]:text-foreground-primary"
                >
                  {period[1]}
                </ToggleGroup.Item>
              ))}
            </ToggleGroup.Root>
          </Container>

          <Span className="flex flex-col md:flex-row md:items-center gap-4">
            <Span className="text-5xl xl:text-9xl font-extrabold truncate">
              {!now ? (
                <Container className="flex items-center h-32">
                  <Span className="bg-background-surface rounded-full h-10 w-sm animate-pulse flex" />
                </Container>
              ) : (
                formatCurrency(hoveredValue ?? getValue(now))
              )}
            </Span>
            {diff != 0 && (
              <Span className={clsx("inline-flex items-center text-xl xl:text-2xl", diff > 0 ? "text-green" : "text-red")}>
                {diff > 0 ? <Icon symbol="arrow_drop_up" size={40} /> : <Icon symbol="arrow_drop_down" size={40} />}{" "}
                <Span>{(Math.abs(diff) * 100).toFixed(2)}%</Span>{" "}
              </Span>
            )}
          </Span>
        </Container>
        <ResponsiveContainer className="" width="100%" aspect={2.5} maxHeight={256}>
          <Container className="relative h-full w-full">
            <LineChart
              accessibilityLayer
              data={combinedData}
              margin={{ left: 12, right: 12 }}
              onMouseMove={(state: any) => {
                const index = state?.activeTooltipIndex;

                if (index == null || index >= combinedData.length) {
                  if (point !== null) {
                    setPoint(null);
                  }

                  return;
                }

                if (index == null) {
                  if (point !== null) {
                    setPoint(null);
                  }

                  return;
                }

                if (index !== point) {
                  setPoint(index);
                }
              }}
              onMouseLeave={() => {
                if (point !== null) {
                  setPoint(null);
                }
              }}
            >
              <XAxis hide dataKey="date" type="number" scale="time" domain={["dataMin", "dataMax"]} tick={false} />
              <YAxis hide domain={["auto", "auto"]} />
              <Line
                type="monotone"
                dataKey="historyValue"
                stroke={color}
                strokeWidth={2}
                dot={false}
                activeDot={false}
                isAnimationActive={false}
                connectNulls
              />
              <Line
                type="monotone"
                dataKey="forecastValue"
                stroke={"var(--color-line-base)"}
                strokeWidth={2}
                strokeOpacity={0.25}
                dot={false}
                activeDot={false}
                isAnimationActive={false}
                connectNulls
              />

              <Tooltip
                content={() => null}
                cursor={{
                  stroke: color,
                  strokeWidth: 2,
                  strokeDasharray: "2 6"
                }}
              />
            </LineChart>
          </Container>
        </ResponsiveContainer>
      </Container>
    </Container>
  );
});

const Ethereum = createElement<typeof Svg>((props, ref) => {
  return (
    <Svg {...props} ref={ref} xmlns="http://www.w3.org/2000/svg" version="1.1" viewBox="0 0 540 879.4" fill="currentColor">
      <Path d="m269.9 325.2-269.9 122.7 269.9 159.6 270-159.6z" />
      <Path d="m0.1 447.8 269.9 159.6v-607.4z" />
      <Path d="m270 0v607.4l269.9-159.6z" />
      <Path d="m0 499 269.9 380.4v-220.9z" />
      <Path d="m269.9 658.5v220.9l270.1-380.4z" />
    </Svg>
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

function formatDate(date?: Date | null, period?: keyof typeof PERIODS, isHovering?: boolean) {
  if (!isHovering) return "Live";
  if (!date) {
    return "";
  }

  if (period === "24h") {
    return new Intl.DateTimeFormat("en-US", {
      hour: "2-digit",
      minute: "2-digit",
      hour12: false
    }).format(date);
  }

  if (period === "7d") {
    return new Intl.DateTimeFormat("en-US", {
      weekday: "short",
      hour: "2-digit",
      minute: "2-digit",
      hour12: false
    }).format(date);
  }

  if (period === "30d") {
    return new Intl.DateTimeFormat("en-US", {
      month: "short",
      day: "numeric"
    }).format(date);
  }

  return new Intl.DateTimeFormat("en-US", {
    month: "short",
    year: "numeric"
  }).format(date);
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
