// Copyright © Spatial Corporation. All rights reserved.

import { Record } from "..";

export type Account = Record<{
  /**
   * The user's name.
   */
  name: string;

  /**
   * The user's email address.
   */
  email: string;

  /**
   * The user's avatar.
   */
  avatar?: string;
}>;
