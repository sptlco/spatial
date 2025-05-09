// Copyright © Spatial. All rights reserved.

import { ChartParameter } from "..";

/**
 * Configurable options for a chart.
 */
export type ChartConfiguration = {
  /**
   * Optional parameters for the chart.
   */
  parameters: ChartParameter[];

  /**
   * Optional data to visualize.
   */
  data?: any[];
};
