// Copyright © Spatial Corporation. All rights reserved.

import { ElementProps } from "../..";

/**
 * Configurable options for a tooltip provider element.
 */
export type TooltipProviderProps = ElementProps & {
  /**
   * The number of milliseconds before a tooltip opens.
   */
  delayDuration?: number;

  /**
   * How much time a user has to enter another trigger without
   * incurring a delay.
   */
  skipDelayDuration?: number;

  /**
   * Whether or not to prevent the tooltip content from remaining
   * open when hovering.
   */
  disableHoverableContent?: boolean;
};
