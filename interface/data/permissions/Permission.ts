// Copyright © Spatial Corporation. All rights reserved.

import { Resource } from "..";

export type Permission = Resource<{
  /**
   * The role granted the permission.
   */
  role: string;

  /**
   * The scope accessible by the role.
   */
  scope: string;
}>;
