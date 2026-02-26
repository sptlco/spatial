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

  const selector = () => {
    return (
      <Select.Root value={year.toString()} onValueChange={(value) => setYear(Number(value))}>
        <Select.Trigger asChild>
          <Button intent="ghost" shape="pill" className="data-[state=open]:bg-button-ghost-active px-4! font-semibold! text-hint!">
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
                label={value.toString()}
                description={`${s.toLocaleDateString(undefined, { timeZone: "UTC", month: "short", day: "2-digit", year: "numeric" })} - ${e.toLocaleDateString(undefined, { timeZone: "UTC", month: "short", day: "2-digit", year: "numeric" })}`}
              />
            );
          })}
        </Select.Content>
      </Select.Root>
    );
  };

  const history = useSWR(["profits", year], () => Spatial.metrics.read("ethereum", start, end, undefined), {
    refreshInterval: 10000,
    dedupingInterval: 15000
  });

  if (!history.data || history.data.error || history.data.data.length < 2) {
    return (
      <Container
        {...props}
        key="spinner"
        ref={ref}
        className={clsx("flex flex-col text-hint items-center justify-center xl:basis-[738px]", props.className)}
      >
        <Container className="flex flex-col items-center justify-center p-10 aspect-square gap-8 bg-background-surface rounded-4xl">
          <Span className="font-semibold">Profit / Loss</Span>
          {selector()}
          <Spinner className="size-5" />
        </Container>
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

    map.set(key, (map.get(key) ?? 0) + delta);
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

  const first = metrics[0];
  const latest = metrics[metrics.length - 1];

  const total = metrics.slice(1).reduce((sum, m, i) => sum + (equity(m) - equity(metrics[i])), 0);
  const diff = (equity(latest) - equity(first)) / equity(first);
  const max = Math.max(...weeks.flat().map((d) => Math.abs(d.value)), 1);

  function getColor(value: number) {
    if (value === 0) {
      return "bg-input";
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
      className={clsx("flex flex-col xl:h-full justify-between w-screen xl:w-auto duration-500 animate-in fade-in zoom-in-95", props.className)}
    >
      <Container className="flex flex-col justify-between p-10 bg-background-surface xl:rounded-[56px] gap-10">
        <Container className="flex flex-col w-full gap-10">
          <H2 className="text-2xl inline-flex items-center gap-5 font-bold">
            <Span>Profit / Loss</Span>
            {selector()}
          </H2>

          <Span className={clsx("flex flex-col md:flex-row md:items-center gap-4", total > 0 ? "text-green" : "text-red")}>
            <Span className={clsx("text-4xl xl:text-8xl font-extrabold truncate")}>{formatCurrency(total)}</Span>
            {diff != 0 && (
              <Span className={clsx("inline-flex items-center text-xl xl:text-2xl")}>
                {diff > 0 ? <Icon symbol="arrow_drop_up" size={40} /> : <Icon symbol="arrow_drop_down" size={40} />}{" "}
                <Span>{(Math.abs(diff) * 100).toFixed(2)}%</Span>{" "}
              </Span>
            )}
          </Span>
        </Container>
        <Container className="w-full grid grid-cols-21 gap-1 xl:hidden">
          {days.map((day, i) => (
            <Tooltip.Root key={i} delayDuration={0}>
              <Tooltip.Trigger>
                <Container
                  suppressHydrationWarning
                  className={clsx("rounded-xs aspect-square w-full transition-colors duration-200", day.date ? getColor(day.value) : "bg-input")}
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
        <Container className="hidden xl:flex gap-1">
          <Container className="grid grid-rows-8 gap-1 text-xs text-foreground-quaternary font-semibold">
            {["", "", "Mon", "", "Wed", "", "Fri", ""].map((label, i) => (
              <Span key={i} className="flex h-2.5 items-center">
                {label}
              </Span>
            ))}
          </Container>
          <Container className="flex flex-col gap-1">
            <Container className="grid grid-cols-12 gap-1 text-xs text-foreground-quaternary font-semibold">
              {[...Array(12)].map((_, i) => (
                <Span key={i} className="flex h-2.5 items-center">
                  {new Date(year, (start.getUTCMonth() + i) % 12).toLocaleDateString(undefined, { month: "short" })}
                </Span>
              ))}
            </Container>
            <Container className="grid grid-rows-7 grid-flow-col gap-1">
              {weeks.map((days, column) =>
                days.map((day, row) => (
                  <Tooltip.Root key={`${column}-${row}`} delayDuration={0} disableHoverableContent>
                    <Tooltip.Trigger>
                      <Container
                        suppressHydrationWarning
                        className={clsx("rounded-xs size-2.5 transition-colors duration-200", day.date ? getColor(day.value) : "bg-input")}
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
