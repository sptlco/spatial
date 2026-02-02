// Copyright © Spatial Corporation. All rights reserved.

import { Resource } from "..";

export type Role = Resource<{
  /**
   * The role's display name.
   */
  name: string;

  /**
   * What users with this role do.
   */
  description: string;

  /**
   * The color associated with the role.
   */
  color: string;
}>;
