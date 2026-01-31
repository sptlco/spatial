// Copyright © Spatial Corporation. All rights reserved.

import { User } from "@sptlco/data";
import { Controller } from "..";

/**
 * A controller for users.
 */
export class UserController extends Controller {
  /**
   * Get the current user.
   * @returns The current user.
   */
  public current = async () => {
    return this.get<User>("users/current");
  };

  /**
   * Get a list of users.
   * @returns A list of users.
   */
  public list = async () => {
    return this.get<User[]>("users/list");
  };
}
