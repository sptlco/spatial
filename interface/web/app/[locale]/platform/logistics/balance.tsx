// Copyright © Spatial Corporation. All rights reserved.

"use client";

import { Spatial } from "@sptlco/client";
import { createElement, Span } from "@sptlco/design";
import useSWR from "swr";

/**
 * An element that displays the current account balance.
 */
export const Balance = createElement<typeof Span>((props, ref) => {
  const { data, isLoading } = useSWR("platform/logistics/assets/balance", Spatial.market.balance, {
    refreshInterval: 30000, // only poll every 30s
    dedupingInterval: 15000
  });

  const balance = data && !data.error ? data.data : undefined;

  return isLoading ? (
    <Span className="flex flex-col gap-2">
      <Span className="rounded-full flex bg-translucent w-1/3 h-10 ml-10 animate-pulse" />
      <Span className="rounded-full flex bg-translucent w-3/4 h-32 animate-pulse" />
    </Span>
  ) : (
    <Span className="w-full flex flex-col gap-2">
      <Span className="text-sm xl:text-2xl font-bold">Balance</Span>
      <Span className="text-2xl xl:text-9xl font-extrabold xl:-ml-2 truncate">{balance} ETH</Span>
    </Span>
  );
});
