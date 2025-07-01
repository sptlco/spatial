// Copyright © Spatial Corporation. All rights reserved.

import { ElementProps } from "../..";

/**
 * Configurable options for a hover card trigger element.
 */
export type HoverCardTriggerProps = ElementProps & {
  /**
   * Whether or not to render the trigger as a child.
   */
  asChild?: boolean;
};
