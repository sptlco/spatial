// Copyright © Spatial Corporation. All rights reserved.

import { Asset } from "./Asset";
import Stripe from "stripe";

/**
 * A detailed view of an {@link Asset}.
 */
export type AssetView = {
  asset: Asset;
  model: Stripe.Product;
};
