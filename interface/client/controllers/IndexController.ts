// Copyright © Spatial Corporation. All rights reserved.

import { Controller } from "..";

/**
 * A root controller.
 */
export class IndexController extends Controller {
  /**
   * Get the server's Ethereum address.
   * @returns The server's Ethereum address.
   */
  public address = () => this.get<string>("address");

  /**
   * Get the server's name.
   * @returns The server's name.
   */
  public name = () => this.get<string>("name");

  /**
   * Get the server's current version.
   * @returns The server's current version.
   */
  public version = () => this.get<string>("version");
}
