// Copyright © Spatial Corporation. All rights reserved.

import { Parcel } from "..";

/**
 * A logical grouping of {@link Parcel} documents sharing a common shipment
 * identifier. Not persisted directly — derived at query time.
 */
export type Shipment = {
  /**
   * The shared identifier across all parcels in this shipment.
   */
  id: string;

  /**
   * The parcels that make up this shipment.
   */
  packages: Parcel[];
};
