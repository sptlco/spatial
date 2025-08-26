// Copyright © Spatial Corporation. All rights reserved.

import { ChartProps } from "..";

/**
 * Configurable options for a bar chart element.
 */
export type BarChartProps = ChartProps & {
  /**
   * The chart's axis key.
   */
  axis: string;

  /**
   * An optional formatter for the axis.
   */
  axisFormatter?: (value: any, index: number) => string;
};
