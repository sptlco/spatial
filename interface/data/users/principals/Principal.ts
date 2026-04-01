// Copyright © Spatial Corporation. All rights reserved.

/**
 * An identity for the current request used for all authorization decisions.
 */
export type Principal = {
  /**
   * The user's roles.
   */
  roles: string[];

  /**
   * The user's permissions.
   */
  permissions: string[];
};
