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

  model: string;

  lot: string;

  quantity: number;
}>;
