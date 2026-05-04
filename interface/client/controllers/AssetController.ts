// Copyright © Spatial Corporation. All rights reserved.

import { Controller } from "..";
import { Asset, AssetView, CreateAssetOptions } from "@sptlco/data";

/**
 * A {@link Controller} for asset functions.
 */
export class AssetController extends Controller {
  /**
   * Get a list of assets.
   * @returns A list of assets.
   */
  public list = () => this.get<AssetView[]>("assets");

  /**
   * Create a new asset.
   * @returns The created asset.
   */
  public create = (options: CreateAssetOptions) => this.post<AssetView>("assets", options);

  /**
   * Update an {@link Asset}.
   * @returns The updated {@link Asset}.
   */
  public update = (asset: Asset) => this.patch<Asset>("assets", asset);
}
