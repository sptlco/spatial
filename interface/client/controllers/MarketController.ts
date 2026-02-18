// Copyright © Spatial Corporation. All rights reserved.

import { Coin } from "@sptlco/data";
import { Controller } from "..";

/**
 * A controller for market functions.
 */
export class MarketController extends Controller {
  /**
   * Fetch the current market data.
   * @returns The current market data.
   */
  public current = async () => {
    return this.get<Coin>("market");
  };

  /**
   * Get the current account balance.
   * @returns The current account balance.
   */
  public balance = async () => {
    return this.get<string>("market/balance");
  };
}
