// Copyright © Spatial Corporation. All rights reserved.

import { Schema } from "../../../..";

/**
 * A user role assignment.
 */
export type Assignment = Schema<{
  /**
   * The user the role is assigned to.
   */
  user: string;

  /**
   * The role that was assigned.
   */
  role: string;
}>;
