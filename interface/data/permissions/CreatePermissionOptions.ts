// Copyright © Spatial Corporation. All rights reserved.

/**
 * Configurable options for a new permission.
 */
export type CreatePermissionOptions = {
  /**
   * The role the permission is granted to.
   */
  role: string;

  /**
   * The scope to grant access to.
   */
  scope: string;
};
