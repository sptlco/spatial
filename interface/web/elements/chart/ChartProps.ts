// Copyright © Spatial Corporation. All rights reserved.

import { ChartConfiguration, ElementProps } from "..";

/**
 * Configurable options for a chart.
 */
export type ChartProps<TConfiguration = ChartConfiguration> = ElementProps & {
  /**
   * Configurable options for the chart.
   */
  config: TConfiguration;
};
