// Copyright © Spatial Corporation. All rights reserved.

import { CreateResourceOptions } from "..";

/**
 * Configurable options for a new role.
 */
export type CreateRoleOptions = CreateResourceOptions & {
  /**
   * The name of the role.
   */
  name: string;

  /**
   * A color associated with the role.
   */
  color: string;

  /**
   * A message describing what the role does.
   */
  description?: string;
};
