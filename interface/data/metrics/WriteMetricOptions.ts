// Copyright © Spatial Corporation. All rights reserved.

/**
 * Configurable options for a new metric.
 */
export type WriteMetricOptions = {
  /**
   * Contextual data about the metric..
   */
  metadata: any;

  /**
   * The metric's value.
   */
  value: Record<string, number>;

  /**
   * The time the metric occurred.
   */
  timestamp: Date;
};
