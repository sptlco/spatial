// Copyright © Spatial. All rights reserved.

import { BadgeIntent, ElementProps } from "..";

/**
 * Configurable options for a badge element.
 */
export type BadgeProps = ElementProps & {
  /**
   * The badge's intent.
   */
  intent?: BadgeIntent;

  /**
   * The badge's roundness.
   */
  roundness?: "round" | "pill";
};
