// Copyright © Spatial Corporation. All rights reserved.

import { Address, AssetType, CreateResourceOptions } from "..";

export type CreateAssetOptions = CreateResourceOptions & {
  type: AssetType;
  model: string;
  product: string;
  lot?: string;
  quantity?: number;
  location?: Address;
};
