// Copyright © Spatial Corporation. All rights reserved.

import { Address, CreateResourceOptions } from "..";

export type CreateShipmentOptions = CreateResourceOptions & {
  from: Address;
  to: Address;
};
