// Copyright © Spatial Corporation. All rights reserved.

import { Address, AssetType, Schema } from "..";

/**
 * A physical or digital good.
 */
export type Asset = Schema<{
  /**
   * The asset's model identifier.
   */
  model: string;

  /**
   * The asset's {@link AssetType}.
   */
  type: AssetType;

  /**
   * The amount of the asset on hand.
   */
  quantity: number;

  /**
   * The asset's physical location.
   */
  location?: Address;

  /**
   * Configurable options for the asset.
   */
  variants: Record<string, string>;
}>;
