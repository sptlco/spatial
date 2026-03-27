// Copyright © Spatial Corporation. All rights reserved.

import { AccountController } from "./AccountController";
import { AssignmentController } from "./AssignmentController";
import { IndexController } from "./IndexController";
import { KeyController } from "./KeyController";
import { MetricController } from "./MetricController";
import { ParcelController } from "./ParcelController";
import { PermissionController } from "./PermissionController";
import { RoleController } from "./RoleController";
import { ScopeController } from "./ScopeController";
import { SessionController } from "./SessionController";
import { ShipmentController } from "./ShipmentController";
import { UserController } from "./UserController";

export * from "./AccountController";
export * from "./AssignmentController";
export * from "./IndexController";
export * from "./KeyController";
export * from "./MetricController";
export * from "./ParcelController";
export * from "./PermissionController";
export * from "./RoleController";
export * from "./ScopeController";
export * from "./SessionController";
export * from "./ShipmentController";
export * from "./UserController";

export const Spatial = {
  ...new IndexController(),
  accounts: new AccountController(),
  assignments: new AssignmentController(),
  keys: new KeyController(),
  metrics: new MetricController(),
  parcels: new ParcelController(),
  permissions: new PermissionController(),
  roles: new RoleController(),
  scopes: new ScopeController(),
  sessions: new SessionController(),
  shipments: new ShipmentController(),
  users: new UserController()
};
