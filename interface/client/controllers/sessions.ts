// Copyright © Spatial Corporation. All rights reserved.

import { Controller, CreateSessionOptions } from "..";

/**
 * A controller for sessions.
 */
export class SessionController extends Controller {
  /**
   * Create a new session.
   * @param options Configurable options for the request.
   * @returns An authorization token.
   */
  public create = async (options: CreateSessionOptions) => {
    return this.post<string>("sessions", options);
  };
}
