// Copyright © Spatial Corporation. All rights reserved.

import { Node } from "../..";

/**
 * Configurable options for a radial chart.
 */
export type RadialChartConfiguration = {
  /**
   * The name of the chart data.
   */
  name: string;

  /**
   * The chart's color.
   */
  color: string;

  /**
   * The chart's maximum value.
   */
  max?: number;

  /**
   * An optional label.
   */
  label?: string;

  /**
   * An optional formatter.
   * @param value The chart's value.
   * @param maxValue The chart's maximum value.
   * @returns A node.
   */
  formatter?: (value: number, maxValue: number) => string;
};
