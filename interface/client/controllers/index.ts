// Copyright © Spatial Corporation. All rights reserved.

import { AccountController } from "./AccountController";
import { IndexController } from "./IndexController";
import { KeyController } from "./KeyController";
import { PermissionController } from "./PermissionController";
import { RoleController } from "./RoleController";
import { ScopeController } from "./ScopeController";
import { SessionController } from "./SessionController";
import { UserController } from "./UserController";

export * from "./AccountController";
export * from "./IndexController";
export * from "./KeyController";
export * from "./PermissionController";
export * from "./RoleController";
export * from "./ScopeController";
export * from "./SessionController";
export * from "./UserController";

export const Spatial = {
  ...new IndexController(),
  accounts: new AccountController(),
  keys: new KeyController(),
  permissions: new PermissionController(),
  roles: new RoleController(),
  scopes: new ScopeController(),
  sessions: new SessionController(),
  users: new UserController()
};
