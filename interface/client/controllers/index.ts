// Copyright © Spatial Corporation. All rights reserved.

import { KeyController } from "./KeyController";
import { SessionController } from "./SessionController";

export * from "./KeyController";
export * from "./SessionController";

export const Spatial = {
  keys: new KeyController(),
  sessions: new SessionController()
};
