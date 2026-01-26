// Copyright © Spatial Corporation. All rights reserved.

import { Resource } from "..";

/**
 * A user role assignment.
 */
export type Assignment = Resource<{
  /**
   * The user the role is assigned to.
   */
  user: string;

  /**
   * The role that was assigned.
   */
  role: string;
}>;
