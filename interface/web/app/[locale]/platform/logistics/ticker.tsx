// Copyright © Spatial Corporation. All rights reserved.

"use client";

import { Spatial } from "@sptlco/client";
import { Container, createElement, Span } from "@sptlco/design";
import useSWR from "swr";

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

function formatNumber(value?: number) {
  if (value == null) return "-";
  return new Intl.NumberFormat("en-US").format(value);
}

/**
 * An animated element that displays market data.
 */
export const Ticker = createElement<typeof Container>((props, ref) => {
  const { data, isLoading, error } = useSWR("platform/logistics/ticker/market", Spatial.market.current);

  const coin = data && !data.error ? data.data : undefined;
  const positive = (coin?.price_change_percentage_24h ?? 0) >= 0;

  const content = (
    <Container className="flex gap-5 xl:gap-10 items-center pr-5 xl:pr-10">
      {isLoading && <Span>Loading Ethereum</Span>}
      {error && <Span className="text-red">Market data unavailable</Span>}
      {coin && (
        <>
          <Span>{coin.symbol?.toUpperCase()}</Span>
          <Span>Price {formatCurrency(coin.current_price)}</Span>
          <Span className={positive ? "text-green" : "text-red"}>
            24h {formatNumber(coin.price_change_24h)} ({formatNumber(coin.price_change_percentage_24h)}%)
          </Span>
          <Span>Market Cap {formatCurrency(coin.market_cap)}</Span>
          <Span>Volume {formatCurrency(coin.total_volume)}</Span>
        </>
      )}
    </Container>
  );

  return (
    <Container {...props} ref={ref} className="relative w-full h-10 overflow-hidden xl:mask-l-from-80% xl:mask-r-from-80%">
      <Container className="absolute inset-0 w-max ticker-track whitespace-nowrap flex text-2xl xl:text-4xl font-extrabold uppercase">
        {content}
        {content}
      </Container>
    </Container>
  );
});
