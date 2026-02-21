// Copyright © Spatial Corporation. All rights reserved.

"use client";

import { Spatial } from "@sptlco/client";
import { createElement, Span } from "@sptlco/design";
import useSWR from "swr";

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
  const market = useSWR("platform/logistics/ticker/market", Spatial.market.current);

  const { data, isLoading } = useSWR("platform/logistics/assets/balance", Spatial.market.balance, {
    refreshInterval: 30000,
    dedupingInterval: 15000
  });

  const balance = data && !data.error ? data.data : undefined;
  const ethereum = market.data && !market.data.error ? market.data.data : undefined;

  return isLoading ? (
    <Span className="flex flex-col gap-2 px-10">
      <Span className="rounded-full flex bg-translucent w-1/3 h-10 xl:ml-10 animate-pulse" />
      <Span className="rounded-full flex bg-translucent w-full xl:w-3/4 h-10 xl:h-32 animate-pulse" />
    </Span>
  ) : (
    <Span {...props} ref={ref} className="w-full flex flex-col gap-2 px-10">
      <Span className="text-xs xl:text-xl font-extrabold uppercase">Balance</Span>
      <Span className="text-2xl xl:text-9xl font-extrabold xl:-ml-2 truncate">
        {formatCurrency(Number(balance) * (ethereum?.current_price ?? 0))}
      </Span>
    </Span>
  );
});
