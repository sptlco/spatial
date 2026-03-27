// Copyright © Spatial Corporation. All rights reserved.

/**
 * A good contained within a {@link Parcel}.
 */
export type Commodity = {
  /**
   * The good's unit identifier.
   */
  unit: string;

  /**
   * The amount of the good being shipped.
   */
  quantity: number;
};
