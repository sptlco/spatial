// Copyright © Spatial Corporation. All rights reserved.

import { AccountController } from "./AccountController";
import { IndexController } from "./IndexController";
import { KeyController } from "./KeyController";
import { PrincipalController } from "./PrincipalController";
import { SessionController } from "./SessionController";

export * from "./AccountController";
export * from "./IndexController";
export * from "./KeyController";
export * from "./PrincipalController";
export * from "./SessionController";

export const Spatial = {
  ...new IndexController(),
  accounts: new AccountController(),
  keys: new KeyController(),
  principals: new PrincipalController(),
  sessions: new SessionController()
};
