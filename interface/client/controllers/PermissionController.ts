// Copyright © Spatial Corporation. All rights reserved.

import { Permission } from "@sptlco/data";
import { Controller } from "..";

/**
 * A controller for permissions.
 */
export class PermissionController extends Controller {
  /**
   * Get a list of permissions.
   * @returns A list of permissions.
   */
  public list = async () => {
    return await this.get<Permission[]>("permissions/list");
  };
}
