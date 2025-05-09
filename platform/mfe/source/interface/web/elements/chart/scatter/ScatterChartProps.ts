// Copyright © Spatial. All rights reserved.

import { ChartProps } from "..";

/**
 * Configurable options for a scatter chart element.
 */
export type ScatterChartProps = ChartProps & {
  /**
   * The chart's axis key
   */
  axis: string;

  /**
   * An optional formatter for the axis.
   */
  axisFormatter?: (value: any, index: number) => string;
};
