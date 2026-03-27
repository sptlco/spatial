// Copyright © Spatial Corporation. All rights reserved.

import { Address, Commodity, CreateResourceOptions, Dimensions, ParcelStatus } from "..";

/**
 * Configurable options for a new {@link Parcel}.
 */
export type CreateParcelOptions = CreateResourceOptions & {
  /**
   * The package's origin {@link Address}.
   */
  origin: Address;

  /**
   * The package's destination {@link Address}.
   */
  destination: Address;

  /**
   * The carrier responsible for the package.
   */
  carrier: string;

  /**
   * The package's tracking number.
   */
  trackingNumber: string;

  /**
   * The current status of the package.
   */
  status: ParcelStatus;

  /**
   * The package's weight in grams.
   */
  weight: number;

  /**
   * The package's physical dimensions.
   */
  dimensions: Dimensions;

  /**
   * The items in the package.
   */
  units: Commodity[];
};
