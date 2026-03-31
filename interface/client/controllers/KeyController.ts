// Copyright © Spatial Corporation. All rights reserved.

import { CreateKeyOptions } from "@sptlco/data";
import { Controller } from "..";

/**
 * A controller for keys.
 */
export class KeyController extends Controller {
  /**
   * Create a new key.
   * @param options Configurable options for the request.
   * @returns A key identifier.
   */
  public create = (options: CreateKeyOptions) => this.post("keys", options);
}
