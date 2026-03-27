// Copyright © Spatial Corporation. All rights reserved.

import { Address, Commodity, Dimensions, ParcelStatus, Resource } from "..";

/**
 * A physical package containing goods. The primary persisted entity —
 * logical shipments are derived by grouping on {@link shipmentId}.
 */
export type Parcel = Resource<{
  shipmentId: string;
  origin: Address;
  destination: Address;
  carrier?: string;
  trackingId?: string;
  status: ParcelStatus;
  weight?: number;
  dimensions?: Dimensions;
  units: Commodity[];
}>;
