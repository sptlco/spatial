// Copyright © Spatial Corporation. All rights reserved.

import { Address, AssetType, CreateResourceOptions } from "..";

export type CreateAssetOptions = CreateResourceOptions & {
  model: string;
  type: AssetType;
  quantity: number;
  location?: Address;
  variants?: Record<string, string>;
};
