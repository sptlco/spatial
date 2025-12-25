// Copyright © Spatial Corporation. All rights reserved.

import { IndexController } from "./IndexController";
import { KeyController } from "./KeyController";
import { SessionController } from "./SessionController";

export * from "./IndexController";
export * from "./KeyController";
export * from "./SessionController";

export const Spatial = {
  ...new IndexController(),
  keys: new KeyController(),
  sessions: new SessionController()
};
