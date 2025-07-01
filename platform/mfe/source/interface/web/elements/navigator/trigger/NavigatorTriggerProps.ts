// Copyright © Spatial Corporation. All rights reserved.

import { ElementProps } from "../..";

/**
 * Configurable options for a navigator trigger element.
 */
export type NavigatorTriggerProps = ElementProps & {
  /**
   * Whether or not to render the trigger as its children.
   */
  asChild?: boolean;

  /**
   * Whether or not the trigger is disabled.
   */
  disabled?: boolean;

  /**
   * Whether or not to suppress default trigger events.
   */
  suppressEvents?: boolean;
};
