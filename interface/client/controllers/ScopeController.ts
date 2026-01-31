// Copyright © Spatial Corporation. All rights reserved.

import { Sector } from "@sptlco/data";
import { Controller } from "..";

/**
 * A controller for scope functions.
 */
export class ScopeController extends Controller {
  /**
   * Get a list of scopes.
   * @returns A list of scopes.
   */
  public list = () => this.get<Sector[]>("scopes/list");
}
