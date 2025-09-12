import { ChartProps } from "..";

/**
 * Configurable options for a line chart element.
 */
export type LineChartProps = ChartProps & {
  /**
   * The chart's axis key
   */
  axis: string;

  /**
   * An optional formatter for the axis.
   */
  axisFormatter?: (value: any, index: number) => string;
};
