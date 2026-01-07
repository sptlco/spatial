// Copyright © Spatial Corporation. All rights reserved.

import { Record } from "..";

export type Permission = Record<{
  /**
   * The role granted the permission.
   */
  role: string;

  /**
   * The scope accessible by the role.
   */
  scope: string;
}>;
