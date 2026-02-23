// Copyright © Spatial Corporation. All rights reserved.

"use client";

import { Spatial } from "@sptlco/client";
import { clsx } from "clsx";
import useSWR from "swr";

import { Container, createElement, H2, Icon, Span, Spinner, Tooltip } from "@sptlco/design";
import { Metric } from "@sptlco/data";

type Bucket = {
  date: Date | null; // null = outside year padding
  value: number;
};

export const Profits = createElement<typeof Container>((props, ref) => {
  const now = new Date();
  const currentYear = now.getFullYear();
  const currentMonth = now.getMonth();

  const fiscalStartYear = currentMonth < 6 ? currentYear - 1 : currentYear;

  const startOfYear = new Date(fiscalStartYear, 6, 1);
  const endOfYear = new Date(fiscalStartYear + 1, 5, 30);

  const history = useSWR(["profits", fiscalStartYear], () => Spatial.metrics.read("ethereum", startOfYear, undefined, undefined, "1d"), {
    refreshInterval: 10000,
    dedupingInterval: 15000
  });

  if (!history.data || history.data.error) {
    return (
      <Container
        {...props}
        key="spinner"
        ref={ref}
        className={clsx("flex flex-col items-center justify-center basis-[738px] gap-8 rounded-4xl", props.className)}
      >
        <Spinner className="size-5" />
      </Container>
    );
  }

  const metrics: Metric[] = history.data.data ?? [];
  const map = new Map<string, number>();

  const equity = (m: Metric) => m.value.balance * m.value.price;

  metrics.forEach((m, i) => {
    if (i === 0) {
      return;
    }

    const prev = metrics[i - 1];
    const date = new Date(m.timestamp);
    const key = getKey(date);
    const delta = equity(m) - equity(prev);

    map.set(key, delta);
  });

  const buckets: Bucket[] = [];

  // 0 = Monday, 6 = Sunday
  const weekday = (date: Date) => {
    const d = date.getDay();
    return d === 0 ? 6 : d - 1;
  };

  const weeks: Bucket[][] = [];
  let week: Bucket[] = Array(7).fill({ date: null, value: 0 });

  let cursor = new Date(startOfYear);

  while (cursor <= endOfYear) {
    const dayIndex = weekday(cursor); // 0 = Monday, 6 = Sunday
    week[dayIndex] = { date: new Date(cursor), value: map.get(getKey(cursor)) ?? 0 };

    // If Sunday, push the week and start new
    if (dayIndex === 6) {
      weeks.push(week);
      week = Array(7).fill({ date: null, value: 0 });
    }

    cursor.setDate(cursor.getDate() + 1);
  }

  // Push last partial week
  if (week.some((d) => d.date !== null)) {
    weeks.push(week);
  }

  const total = metrics.length >= 2 ? equity(metrics[metrics.length - 1]) - equity(metrics[0]) : 0;
  const max = Math.max(...buckets.map((d) => Math.abs(d.value)), 1);

  function getColor(value: number) {
    if (value === 0) {
      return "bg-background-subtle";
    }

    const intensity = Math.abs(value) / max;

    if (value > 0) {
      if (intensity > 0.75) return "bg-green";
      if (intensity > 0.5) return "bg-green/60";
      if (intensity > 0.25) return "bg-green/30";
      return "bg-green/10";
    } else {
      if (intensity > 0.75) return "bg-red";
      if (intensity > 0.5) return "bg-red/60";
      if (intensity > 0.25) return "bg-red/30";
      return "bg-red/10";
    }
  }

  return (
    <Container
      {...props}
      ref={ref}
      className={clsx("flex flex-col justify-center items-center gap-16 rounded-4xl duration-500 animate-in fade-in zoom-in-95", props.className)}
    >
      <Container className="flex flex-col gap-6">
        <H2 className="text-2xl font-bold">Annual Profit / Loss</H2>
        <Span className={clsx("text-5xl font-extrabold", total > 0 ? "text-green" : "text-red")}>{formatCurrency(total)}</Span>
      </Container>

      <Container className="grid grid-rows-7 grid-flow-col gap-1">
        {weeks.map((week, colIndex) =>
          week.map((day, rowIndex) => (
            <Tooltip.Root key={`${colIndex}-${rowIndex}`} delayDuration={0} disableHoverableContent>
              <Tooltip.Trigger>
                <Container
                  suppressHydrationWarning
                  className={clsx("rounded-xs size-2.5 transition-colors duration-200", day.date ? getColor(day.value) : "bg-background-subtle")}
                />
              </Tooltip.Trigger>
              {day.date && (
                <Tooltip.Content className="flex items-center gap-1">
                  <Span>{day.date.toDateString()}</Span>
                  <Span className={clsx("flex items-center", day.value > 0 ? "text-green" : day.value < 0 ? "text-red" : "text-hint")}>
                    <Span>{formatCurrency(day.value)}</Span>
                  </Span>
                </Tooltip.Content>
              )}
            </Tooltip.Root>
          ))
        )}
      </Container>
    </Container>
  );
});

// ======================================================
// HELPERS
// ======================================================

function getKey(date: Date) {
  const year = date.getFullYear();
  const month = String(date.getMonth() + 1).padStart(2, "0"); // getMonth() is 0-based
  const day = String(date.getDate()).padStart(2, "0");
  return `${year}-${month}-${day}`;
}

function formatCurrency(value?: number) {
  if (value == null) return "-";

  return new Intl.NumberFormat("en-US", {
    style: "currency",
    currency: "USD",
    currencyDisplay: "code",
    currencySign: "accounting",
    maximumFractionDigits: 2
  }).format(value);
}
