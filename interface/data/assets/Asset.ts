// Copyright © Spatial Corporation. All rights reserved.

import { AssetType, Schema } from "..";

/**
 * A physical or digital good.
 */
export type Asset = Schema<{
  /**
   * The asset's {@link AssetType}.
   */
  type: AssetType;

  /**
   * The asset's model identifier.
   */
  model: string;

  /**
   * The asset's lot identifier.
   */
  lot: string;

  /**
   * The amount of the asset on hand.
   */
  quantity: number;
}>;
