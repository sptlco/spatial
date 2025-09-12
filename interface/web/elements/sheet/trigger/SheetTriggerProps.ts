// Copyright © Spatial Corporation. All rights reserved.

import { ElementProps } from "../..";

/**
 * Configurable options for a sheet trigger element.
 */
export type SheetTriggerProps = ElementProps & {
  /**
   * Whether or not to render the trigger as a child.
   */
  asChild?: boolean;
};