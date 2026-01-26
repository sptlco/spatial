// Copyright © Spatial Corporation. All rights reserved.

/**
 * Configurable options for a new role.
 */
export type CreateRoleOptions = {
  /**
   * The name of the role.
   */
  name: string;

  /**
   * A message describing what the role does.
   */
  description: string;

  /**
   * A color associated with the role.
   */
  color?: string;
};
