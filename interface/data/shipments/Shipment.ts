// Copyright © Spatial Corporation. All rights reserved.

import { Address, Resource } from "..";

/**
 * A shipment between two addresses.
 */
export type Shipment = Resource<{
  from: Address;
  to: Address;
}>;
