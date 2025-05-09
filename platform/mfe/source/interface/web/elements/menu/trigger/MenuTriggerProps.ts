// Copyright © Spatial. All rights reserved.

import { ElementProps } from "../..";

/**
 * Configurable options for a menu trigger element.
 */
export type MenuTriggerProps = ElementProps & {
  /**
   * Whether or not to render the trigger as a child.
   */
  asChild?: boolean;

  /**
   * Whether or not the trigger is disabled.
   */
  disabled?: boolean;
};
