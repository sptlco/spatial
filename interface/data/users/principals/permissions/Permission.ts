// Copyright © Spatial Corporation. All rights reserved.

import { Schema } from "../../..";

export type Permission = Schema<{
  /**
   * The role granted the permission.
   */
  role: string;

  /**
   * The scope accessible by the role.
   */
  scope: string;
}>;
