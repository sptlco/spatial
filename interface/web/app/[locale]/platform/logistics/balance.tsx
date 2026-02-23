// Copyright © Spatial Corporation. All rights reserved.

"use client";

import { Spatial } from "@sptlco/client";
import useSWR from "swr";

import { createElement, Span } from "@sptlco/design";

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
