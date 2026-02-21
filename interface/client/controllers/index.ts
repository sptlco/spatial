// Copyright © Spatial Corporation. All rights reserved.

import { AccountController } from "./AccountController";
import { AssignmentController } from "./AssignmentController";
import { IndexController } from "./IndexController";
import { KeyController } from "./KeyController";
import { MarketController } from "./MarketController";
import { MetricController } from "./MetricController";
import { PermissionController } from "./PermissionController";
import { RoleController } from "./RoleController";
import { ScopeController } from "./ScopeController";
import { SessionController } from "./SessionController";
import { UserController } from "./UserController";

export * from "./AccountController";
export * from "./AssignmentController";
export * from "./IndexController";
export * from "./KeyController";
export * from "./MarketController";
export * from "./MetricController";
export * from "./PermissionController";
export * from "./RoleController";
export * from "./ScopeController";
export * from "./SessionController";
export * from "./UserController";

export const Spatial = {
  ...new IndexController(),
  accounts: new AccountController(),
  assignments: new AssignmentController(),
  keys: new KeyController(),
  market: new MarketController(),
  metrics: new MetricController(),
  permissions: new PermissionController(),
  roles: new RoleController(),
  scopes: new ScopeController(),
  sessions: new SessionController(),
  users: new UserController()
};
