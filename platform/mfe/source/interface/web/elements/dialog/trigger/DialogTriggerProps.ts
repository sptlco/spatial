// Copyright © Spatial Corporation. All rights reserved.

import { ElementProps } from "../..";

/**
 * Configurable options for a dialog trigger element.
 */
export type DialogTriggerProps = ElementProps & {
  /**
   * Whether or not to render the trigger as a child.
   */
  asChild?: boolean;
};
