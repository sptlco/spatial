// Copyright © Spatial Corporation. All rights reserved.

import { ChartProps, RadialChartConfiguration } from "..";

/**
 * Configurable options for a radial chart.
 */
export type RadialChartProps = ChartProps<RadialChartConfiguration> & {
  /**
   * The chart's value.
   */
  value?: number;
};
