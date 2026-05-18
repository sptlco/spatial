// Copyright © Spatial Corporation. All rights reserved.

import { Controller } from "..";
import Stripe from "stripe";

/**
 * A {@link Controller} for product functions.
 */
export class ProductController extends Controller {
  /**
   * Get a list of products.
   * @returns A list of products.
   */
  public list = async () => this.get<Stripe.Product[]>("products");
}
