// Copyright © Spatial Corporation. All rights reserved.

import { ElementProps } from "../..";

/**
 * Configurable options for a drawer trigger element.
 */
export type DrawerTriggerProps = ElementProps & {
  /**
   * Whether or not to render the trigger as a child.
   */
  asChild?: boolean;
};
