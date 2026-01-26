// Copyright © Spatial Corporation. All rights reserved.

import { AccountController } from "./AccountController";
import { IndexController } from "./IndexController";
import { KeyController } from "./KeyController";
import { PermissionController } from "./PermissionController";
import { PrincipalController } from "./PrincipalController";
import { RoleController } from "./RoleController";
import { SessionController } from "./SessionController";
import { UserController } from "./UserController";

export * from "./AccountController";
export * from "./IndexController";
export * from "./KeyController";
export * from "./PermissionController";
export * from "./PrincipalController";
export * from "./RoleController";
export * from "./SessionController";
export * from "./UserController";

export const Spatial = {
  ...new IndexController(),
  accounts: new AccountController(),
  keys: new KeyController(),
  permissions: new PermissionController(),
  principals: new PrincipalController(),
  roles: new RoleController(),
  sessions: new SessionController(),
  users: new UserController()
};
