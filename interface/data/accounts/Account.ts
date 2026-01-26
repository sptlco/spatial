// Copyright © Spatial Corporation. All rights reserved.

import { Resource } from "..";

export type Account = Resource<{
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
