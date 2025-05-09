// Copyright © Spatial. All rights reserved.

import { ElementProps } from "..";

/**
 * Configurable options for a progress element.
 */
export type ProgressProps = ElementProps & {
  /**
   * The progress bar's current value.
   */
  value?: number;

  /**
   * The maximum value.
   */
  max?: number;
};
