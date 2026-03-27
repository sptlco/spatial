// Copyright © Spatial Corporation. All rights reserved.

import { Address, Commodity, Dimensions, ParcelStatus, Resource } from "..";

/**
 * A physical package containing goods. The primary persisted entity —
 * logical shipments are derived by grouping on {@link shipment}.
 */
export type Parcel = Resource<{
  /**
   * Groups this package with others belonging to the same shipment.
   */
  shipment: string;

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
   * The items contained in the package.
   */
  units: Commodity[];
}>;
