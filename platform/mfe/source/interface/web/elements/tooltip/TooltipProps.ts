// Copyright © Spatial. All rights reserved.

import { ElementProps } from "..";

/**
 * Configurable options for a tooltip element.
 */
export type TooltipProps = ElementProps & {
  /**
   * Whether or not the tooltip is open.
   */
  open?: boolean;

  /**
   * An optional change event handler.
   * @param open Whether or not the tooltip is open.
   */
  onChange?: (open: boolean) => void;
};
