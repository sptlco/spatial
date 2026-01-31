// Copyright © Spatial Corporation. All rights reserved.

import { Scope } from "./Scope";

export type Sector = {
  /**
   * The name of the sector.
   */
  name: string;

  /**
   * The scopes in the sector.
   */
  scopes: Scope[];
};
