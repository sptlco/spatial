// Copyright © Spatial Corporation. All rights reserved.

export type Coin = {
  /** Contract address */
  contract_address: string;

  /** Coin identifier */
  id: string;

  /** Token symbol */
  symbol: string;

  /** Token name */
  name: string;

  /** Current price (USD) */
  current_price: number;

  /** Raw on-chain balance (BigInteger serialized as string) */
  balance: string;

  /** Token decimals */
  decimals: number;

  /** Market cap */
  market_cap: number;

  /** Market cap rank */
  market_cap_rank?: number | null;

  /** Fully diluted valuation */
  fully_diluted_valuation: number;

  /** 24h volume */
  total_volume: number;

  /** 24h high */
  high_24h: number;

  /** 24h low */
  low_24h: number;

  /** 24h price change */
  price_change_24h: number;

  /** 24h price change % */
  price_change_percentage_24h: number;

  /** 24h market cap change */
  market_cap_change_24h: number;

  /** 24h market cap change % */
  market_cap_change_percentage_24h: number;

  /** Circulating supply */
  circulating_supply: number;

  /** Total supply */
  total_supply: number;

  /** Max supply */
  max_supply?: number | null;

  /** All-time high */
  ath: number;

  /** ATH change % */
  ath_change_percentage: number;

  /** ATH date (ISO string) */
  ath_date: string;

  /** All-time low */
  atl: number;

  /** ATL change % */
  atl_change_percentage: number;

  /** ATL date (ISO string) */
  atl_date: string;

  /** Return on investment */
  roi?: CoinROI | null;

  /** Last updated (ISO string) */
  last_updated: string;
};

export type CoinROI = {
  times: number;
  currency: string;
  percentage: number;
};
