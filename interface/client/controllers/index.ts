// Copyright © Spatial Corporation. All rights reserved.

import { AccountController } from "./AccountController";
import { IndexController } from "./IndexController";
import { KeyController } from "./KeyController";
import { SessionController } from "./SessionController";

export * from "./AccountController";
export * from "./IndexController";
export * from "./KeyController";
export * from "./SessionController";

export const Spatial = {
  ...new IndexController(),
  accounts: new AccountController(),
  keys: new KeyController(),
  sessions: new SessionController()
};
