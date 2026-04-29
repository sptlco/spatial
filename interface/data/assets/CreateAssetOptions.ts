// Copyright © Spatial Corporation. All rights reserved.

import { Address, CreateResourceOptions } from "..";
import { AssetType } from "./AssetType";

export type CreateAssetOptions = CreateResourceOptions & {
  type: AssetType;
  model: string;
  product: string;
  lot?: string;
  quantity?: number;
  location?: Address;
};
