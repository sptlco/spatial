// Copyright © Spatial Corporation. All rights reserved.

import { CreateParcelOptions } from "..";

/**
 * Configurable options for a new shipment.
 */
export type CreateShipmentOptions = {
  /**
   * The packages to ship.
   */
  packages: CreateParcelOptions[];
};
