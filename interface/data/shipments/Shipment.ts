// Copyright © Spatial Corporation. All rights reserved.

import { Address, Parcel } from "..";

/**
 * A logical grouping of {@link Parcel} documents sharing a common shipment
 * identifier. Not persisted directly — derived at query time.
 */
export type Shipment = {
  /** The shared identifier across all parcels in this shipment. */
  id: string;
  /** Origin address, taken from the first parcel. */
  from: Address;
  /** Destination address, taken from the first parcel. */
  to: Address;
  /** All parcels belonging to this shipment. */
  parcels: Parcel[];
};
