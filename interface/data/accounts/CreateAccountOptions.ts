// Copyright © Spatial Corporation. All rights reserved.

import { CreateResourceOptions } from "..";

/**
 * Configurable options for a new account.
 */
export type CreateAccountOptions = CreateResourceOptions & {
  /**
   * The user's name.
   */
  name: string;

  /**
   * The user's email address.
   */
  email: string;

  /**
   * Configurable options for the account.
   */
  metadata?: Record<string, string>;
};
