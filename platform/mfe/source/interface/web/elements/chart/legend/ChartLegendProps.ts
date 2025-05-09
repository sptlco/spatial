// Copyright © Spatial. All rights reserved.

import { ElementProps } from "../..";

/**
 * Configurable options for a chart legend elemend.
 */
export type ChartLegendProps = ElementProps & {
  /**
   * The data passed to the tooltip.
   */
  payload?: any[];
};
