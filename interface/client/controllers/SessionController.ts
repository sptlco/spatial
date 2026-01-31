// Copyright © Spatial Corporation. All rights reserved.

import { CreateSessionOptions, Session } from "@sptlco/data";
import { Controller } from "..";

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
    return this.post<Session>("sessions", options);
  };

  /**
   * Destroy the current session.
   */
  public destroy = async () => {
    this.delete("sessions/current");
  };
}
