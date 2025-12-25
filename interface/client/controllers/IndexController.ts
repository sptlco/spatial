// Copyright © Spatial Corporation. All rights reserved.

import { Account } from "@sptlco/data";
import { Controller } from "..";

/**
 * A root controller.
 */
export class IndexController extends Controller {
  /**
   * Get the current user's account.
   * @returns The current user's account.
   */
  public me = async () => {
    return this.get<Account>("me");
  };

  /**
   * Get the server's name.
   * @returns The server's name.
   */
  public name = async () => {
    return this.get<string>("name");
  };

  /**
   * Get the server's current version.
   * @returns The server's current version.
   */
  public version = async () => {
    return this.get<string>("version");
  };
}
