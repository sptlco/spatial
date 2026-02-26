// Copyright © Spatial Corporation. All rights reserved.

"use client";

import { Spatial } from "@sptlco/client";
import useSWR from "swr";

import { createElement, Span } from "@sptlco/design";

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

/**
 * An element that displays the current account balance.
 */
export const Balance = createElement<typeof Span>((props, ref) => {
  const history = useSWR("platform/logistics/balance/history", () => Spatial.metrics.read("ethereum"), {
    refreshInterval: 10000,
    dedupingInterval: 15000
  });

  const metrics = history.data && !history.data.error ? history.data.data : undefined;
  const now = metrics && metrics[metrics.length - 1];

  return (
    <Span {...props} ref={ref} className="w-full flex flex-col gap-2 px-10">
      <Span className="text-xs xl:text-xl font-extrabold uppercase">Balance</Span>
      {history.isLoading || !now ? (
        <Span className="rounded-full flex bg-translucent w-full xl:w-1/4 h-10 xl:h-16 xl:my-8 animate-pulse" />
      ) : (
        <Span className="text-2xl xl:text-9xl font-extrabold xl:-ml-2 truncate">{formatCurrency(now.value.balance * now.value.price)}</Span>
      )}
    </Span>
  );
});
