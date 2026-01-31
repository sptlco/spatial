// Copyright © Spatial Corporation. All rights reserved.

import { Difference, Permission } from "@sptlco/data";
import { Controller } from "..";

/**
 * A controller for permissions.
 */
export class PermissionController extends Controller {
  /**
   * Update the permission table.
   * @param difference The update to the table.
   * @returns The updated permission table.
   */
  public update = async (update: Difference<{ role: string; scope: string }>) => {
    return await this.patch<Permission[]>("permissions", update);
  };

  /**
   * Get a list of permissions.
   * @returns A list of permissions.
   */
  public list = async () => {
    return await this.get<Permission[]>("permissions/list");
  };
}
