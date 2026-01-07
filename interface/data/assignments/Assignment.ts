// Copyright © Spatial Corporation. All rights reserved.

import { Record } from "..";

/**
 * A user role assignment.
 */
export type Assignment = Record<{
  /**
   * The user the role is assigned to.
   */
  user: string;

  /**
   * The role that was assigned.
   */
  role: string;
}>;
