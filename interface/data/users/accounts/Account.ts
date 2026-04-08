// Copyright © Spatial Corporation. All rights reserved.

import { Schema } from "../..";

export type Account = Schema<{
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
