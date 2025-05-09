// Copyright © Spatial. All rights reserved.

import { ChartProps } from "..";

/**
 * Configurable options for an area chart element.
 */
export type AreaChartProps = ChartProps & {
  /**
   * The chart's axis key
   */
  axis: string;

  /**
   * An optional formatter for the axis.
   */
  axisFormatter?: (value: any, index: number) => string;

  /**
   * Whether or not to stack the chart data.
   */
  stack?: boolean;
};
