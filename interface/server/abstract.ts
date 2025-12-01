// Copyright © Spatial Corporation. All rights reserved.

import { KeyController, SessionController } from ".";

/**
 * A cloud server abstraction.
 */
export const Spatial = {
  /**
   * A controller for keys.
   */
  keys: new KeyController(),

  /**
   * A controller for sessions.
   */
  sessions: new SessionController(),
};
