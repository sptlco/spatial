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
  public update = (update: Difference<{ role: string; scope: string }>) => this.patch<Permission[]>("permissions", update);

  /**
   * Get a list of permissions.
   * @returns A list of permissions.
   */
  public list = () => this.get<Permission[]>("permissions/list");
}
