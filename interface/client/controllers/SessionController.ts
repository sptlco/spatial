// Copyright © Spatial Corporation. All rights reserved.

import { CreateSessionOptions, Session } from "@sptlco/data";
import { Controller } from "..";

/**
 * A controller for sessions.
 */
export class SessionController extends Controller {
  /**
   * Get the current session.
   * @returns The current session.
   */
  public me = async () => {
    return this.get<Session>("sessions/me");
  };

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
    this.delete("sessions/me");
  };
}
