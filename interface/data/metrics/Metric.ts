// Copyright © Spatial Corporation. All rights reserved.

import { Resource } from "../Resource";

/**
 * A time-stamped measurement of a specific attribute within the system.
 */
export type Metric = Resource<{
  /**
   * The metric's value.
   */
  value: Record<string, number>;

  /**
   * The time the metric occurred.
   */
  timestamp: Date;
}>;
