// Copyright © Spatial. All rights reserved.

import { ElementProps } from "../..";

/**
 * Configurable options for a popup trigger element.
 */
export type PopupTriggerProps = ElementProps & {
  /**
   * Whether or not to render the trigger as its children.
   */
  asChild?: boolean;
};
