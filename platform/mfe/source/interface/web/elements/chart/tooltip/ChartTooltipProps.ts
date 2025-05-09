// Copyright © Spatial. All rights reserved.

import { ElementProps } from "../..";

/**
 * Configurable options for a chart tooltip element.
 */
export type ChartTooltipProps = ElementProps & {
  /**
   * Whether or not the tooltip is active.
   */
  active?: boolean;

  /**
   * The data passed to the tooltip.
   */
  payload?: any[];

  /**
   * The label to display.
   */
  label?: string;

  /**
   * Whether or not to hide the tooltip's label element.
   */
  hideLabel?: boolean;

  /**
   * A list of data colors.
   */
  colors?: { key: string; color: string }[];

  /**
   * Whether or not to force format (for Scatter plots).
   */
  force?: boolean;

  /**
   * The chart's axis key.
   */
  axis?: string;
};
