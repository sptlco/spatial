// Copyright © Spatial Corporation. All rights reserved.

import { ElementProps } from "../..";

/**
 * Configurable options for a tooltip trigger element.
 */
export type TooltipTriggerProps = ElementProps & {
  /**
   * Whether or not to render the trigger as its children.
   */
  asChild?: boolean;
};
