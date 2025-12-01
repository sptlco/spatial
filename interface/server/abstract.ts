// Copyright © Spatial Corporation. All rights reserved.

import { SessionController } from ".";

/**
 * A cloud server abstraction.
 */
export const Spatial = {
  /**
   * A controller for sessions.
   */
  sessions: new SessionController()
};
