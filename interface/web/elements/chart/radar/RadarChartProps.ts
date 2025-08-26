// Copyright © Spatial Corporation. All rights reserved.

import { ChartProps } from "..";

/**
 * Configurable options for a radar chart element.
 */
export type RadarChartProps = ChartProps & {
  /**
   * The chart's axis key.
   */
  axis: string;

  /**
   * An optional formatter for the axis.
   */
  axisFormatter?: (value: any, index: number) => string;
};
