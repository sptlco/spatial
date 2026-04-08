// Copyright © Spatial Corporation. All rights reserved.

import { Schema } from ".";

/**
 * A time-stamped measurement of a specific attribute within the system.
 */
export type Metric = Schema<{
  /**
   * The metric's value.
   */
  value: Record<string, number>;

  /**
   * The time the metric occurred.
   */
  timestamp: Date;
}>;
