// Copyright © Spatial Corporation. All rights reserved.

import { AccountController } from "./AccountController";
import { AssetController } from "./AssetController";
import { AssignmentController } from "./AssignmentController";
import { IndexController } from "./IndexController";
import { KeyController } from "./KeyController";
import { MetricController } from "./MetricController";
import { NeuralController } from "./NeuralController";
import { ParcelController } from "./ParcelController";
import { PermissionController } from "./PermissionController";
import { ProductController } from "./ProductController";
import { RoleController } from "./RoleController";
import { ScopeController } from "./ScopeController";
import { SessionController } from "./SessionController";
import { ShipmentController } from "./ShipmentController";
import { TransmissionController } from "./TransmissionController";
import { UserController } from "./UserController";

export * from "./AccountController";
export * from "./AssetController";
export * from "./AssignmentController";
export * from "./IndexController";
export * from "./KeyController";
export * from "./MetricController";
export * from "./NeuralController";
export * from "./ParcelController";
export * from "./PermissionController";
export * from "./ProductController";
export * from "./RoleController";
export * from "./ScopeController";
export * from "./SessionController";
export * from "./ShipmentController";
export * from "./TransmissionController";
export * from "./UserController";

export const Spatial = {
  ...new IndexController(),
  accounts: new AccountController(),
  assets: new AssetController(),
  assignments: new AssignmentController(),
  brain: new NeuralController(),
  keys: new KeyController(),
  metrics: new MetricController(),
  parcels: new ParcelController(),
  permissions: new PermissionController(),
  products: new ProductController(),
  roles: new RoleController(),
  scopes: new ScopeController(),
  sessions: new SessionController(),
  shipments: new ShipmentController(),
  transmissions: new TransmissionController(),
  users: new UserController()
};
