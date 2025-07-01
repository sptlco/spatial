// Copyright © Spatial Corporation. All rights reserved.

import { ElementProps } from "../..";

/**
 * Configurable options for a dropdown trigger element.
 */
export type DropdownTriggerProps = ElementProps & {
  /**
   * Whether or not to render the trigger as a child.
   */
  asChild?: boolean;
};
