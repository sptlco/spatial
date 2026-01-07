// Copyright © Spatial Corporation. All rights reserved.

import { Record } from "..";

export type Role = Record<{
  /**
   * The role's display name.
   */
  name: string;

  /**
   * What users with this role do.
   */
  description: string;
}>;
