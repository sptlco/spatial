// Copyright © Spatial Corporation. All rights reserved.

"use client";

import { Spatial } from "@sptlco/client";
import { clsx } from "clsx";
import useSWR from "swr";

import { Button, Container, createElement, H2, Icon, Select, Span, Spinner, Tooltip } from "@sptlco/design";
import { Metric } from "@sptlco/data";
import { useState } from "react";

type Bucket = {
  date: Date | null;
  value: number;
};

export const Profits = createElement<typeof Container>((props, ref) => {
  const now = new Date();

  const y = now.getFullYear();
  const m = now.getMonth();

  const [year, setYear] = useState(m < 6 ? y - 1 : y);

  const start = new Date(Date.UTC(year, 6, 1));
  const end = new Date(Date.UTC(year + 1, 6, 1));

  const history = useSWR(["profits", year], () => Spatial.metrics.read("ethereum", start, end, undefined, "1d"), {
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

  const weeks: Bucket[][] = [];

  const weekday = (date: Date) => {
    const day = date.getUTCDay();
    return day === 0 ? 6 : day - 1;
  };

  let week: Bucket[] = Array(7).fill({ date: null, value: 0 });
  let cursor = new Date(start);

  cursor.setUTCDate(cursor.getUTCDate() + 1);

  while (cursor <= end) {
    const day = weekday(cursor);

    week[day] = { date: new Date(cursor), value: map.get(getKey(cursor)) ?? 0 };

    if (day === 6) {
      weeks.push(week);
      week = Array(7).fill({ date: null, value: 0 });
    }

    cursor.setUTCDate(cursor.getUTCDate() + 1);
  }

  if (week.some((d) => d.date !== null)) {
    weeks.push(week);
  }

  const total = metrics.length >= 2 ? equity(metrics[metrics.length - 1]) - equity(metrics[0]) : 0;
  const max = Math.max(...weeks.flat().map((d) => Math.abs(d.value)), 1);

  function getColor(value: number) {
    if (value === 0) {
      return "bg-background-surface";
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

  const days = weeks.flat();

  return (
    <Container
      {...props}
      ref={ref}
      className={clsx(
        "flex flex-col justify-between items-start gap-10 w-screen xl:w-auto xl:rounded-[56px] p-10 bg-background-subtle duration-500 animate-in fade-in zoom-in-95",
        props.className
      )}
    >
      <Container className="flex flex-col w-full gap-10">
        <H2 className="text-2xl inline-flex items-center gap-5 font-bold">
          <Span>Profit / Loss</Span>
          <Select.Root value={year.toString()} onValueChange={(value) => setYear(Number(value))}>
            <Select.Trigger asChild>
              <Button intent="ghost" className="data-[state=open]:bg-button-ghost-active px-4! font-semibold! text-hint!">
                <Span>{year}</Span>
                <Icon symbol="keyboard_arrow_down" />
              </Button>
            </Select.Trigger>
            <Select.Content>
              {[...Array(3)].map((_, i) => {
                const value = (m < 6 ? y - 1 : y) - i;

                const s = new Date(Date.UTC(value, 6, 1));
                const e = new Date(Date.UTC(value + 1, 5, 30));

                return (
                  <Select.Item
                    key={i}
                    value={value.toString()}
                    label={`FY ${value.toString()}`}
                    description={`${s.toLocaleDateString(undefined, { timeZone: "UTC", month: "short", day: "2-digit", year: "numeric" })} - ${e.toLocaleDateString(undefined, { timeZone: "UTC", month: "short", day: "2-digit", year: "numeric" })}`}
                  />
                );
              })}
            </Select.Content>
          </Select.Root>
        </H2>
        <Span className={clsx("text-4xl xl:text-9xl font-extrabold truncate", total > 0 ? "text-green" : "text-red")}>{formatCurrency(total)}</Span>
      </Container>
      <Container className="w-full grid grid-cols-21 gap-1 xl:hidden">
        {days.map((day, i) => (
          <Tooltip.Root key={i} delayDuration={0}>
            <Tooltip.Trigger>
              <Container
                suppressHydrationWarning
                className={clsx(
                  "rounded-xs aspect-square w-full transition-colors duration-200",
                  day.date ? getColor(day.value) : "bg-background-surface"
                )}
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
        ))}
      </Container>
      <Container className="flex gap-2">
        <Container className="grid grid-rows-7 text-xs text-hint">
          {["", "", "Mon", "", "Wed", "", "Fri", ""].map((label, i) => (
            <Span key={i} className="flex h-2.5 leading-[10px]">
              {label}
            </Span>
          ))}
        </Container>
        <Container className="flex flex-col gap-1.5">
          <Container className="w-full grid grid-cols-12 text-foreground-quaternary font-semibold">
            {[...Array(12)].map((_, i) => (
              <Span className="text-xs">{new Date(year, (start.getUTCMonth() + i) % 12).toLocaleDateString(undefined, { month: "short" })}</Span>
            ))}
          </Container>
          <Container className="hidden xl:grid grid-rows-7 grid-flow-col gap-1">
            {weeks.map((days, column) =>
              days.map((day, row) => (
                <Tooltip.Root key={`${column}-${row}`} delayDuration={0} disableHoverableContent>
                  <Tooltip.Trigger>
                    <Container
                      suppressHydrationWarning
                      className={clsx("rounded-xs size-2.5 transition-colors duration-200", day.date ? getColor(day.value) : "bg-background-surface")}
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
      </Container>
    </Container>
  );
});

function getKey(date: Date) {
  const year = date.getUTCFullYear();
  const month = String(date.getUTCMonth() + 1).padStart(2, "0");
  const day = String(date.getUTCDate()).padStart(2, "0");

  return `${year}-${month}-${day}`;
}

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
