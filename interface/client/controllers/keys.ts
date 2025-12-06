// Copyright © Spatial Corporation. All rights reserved.

import { Controller, CreateKeyOptions } from "..";

/**
 * A controller for keys.
 */
export class KeyController extends Controller {
  /**
   * Create a new key.
   * @param options Configurable options for the request.
   * @returns A key identifier.
   */
  public create = async (options: CreateKeyOptions) => {
    return this.post<string>("keys/create", options);
  };
}
