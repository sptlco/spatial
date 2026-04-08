// Copyright © Spatial Corporation. All rights reserved.

import { Schema } from "../../..";

export type Role = Schema<{
  /**
   * The role's display name.
   */
  name: string;

  /**
   * The color associated with the role.
   */
  color: string;

  /**
   * What users with this role do.
   */
  description?: string;
}>;
