// Copyright © Spatial Corporation. All rights reserved.

import { CreateRoleOptions, Role } from "@sptlco/data";
import { Controller } from "..";

/**
 * A controller for role functions.
 */
export class RoleController extends Controller {
  /**
   * Get a list of roles.
   * @returns A list of roles.
   */
  public list = async () => {
    return await this.get<Role[]>("roles/list");
  };

  /**
   * Create a new role.
   * @param options Configurable options for the role.
   * @returns The role.
   */
  public create = async (options: CreateRoleOptions) => {
    return await this.post<Role>("roles", options);
  };

  /**
   * Update a role.
   * @param role A role update.
   * @returns The updated role.
   */
  public update = async (role: Role) => {
    return await this.patch<Role>("roles", role);
  };

  /**
   * Delete a role.
   * @param id The role to delete.
   * @returns A response from the server.
   */
  public del = async (id: string) => {
    return await this.delete(`roles/${id}`);
  };
}
