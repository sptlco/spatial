// Copyright © Spatial Corporation. All rights reserved.

import { Principal } from "@sptlco/data";
import { Controller } from "..";

/**
 * A controller for principals.
 */
export class PrincipalController extends Controller {
  /**
   * Get the current user principal.
   * @returns The current user principal.
   */
  public me = async () => {
    return this.get<Principal>("principals/me");
  };
}
