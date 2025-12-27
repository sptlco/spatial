// Copyright © Spatial Corporation. All rights reserved.

import { Controller } from "..";

/**
 * A root controller.
 */
export class IndexController extends Controller {
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
