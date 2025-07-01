// Copyright © Spatial Corporation. All rights reserved.

import { ElementProps } from "../..";

/**
 * Configurable options for a tooltip content element.
 */
export type TooltipContentProps = ElementProps & {
  /**
   * Whether or not to render the element as its children.
   */
  asChild?: boolean;

  /**
   * The side to render the content on.
   */
  side?: "top" | "bottom" | "left" | "right";

  /**
   * The content's alignment.
   */
  align?: "start" | "center" | "end";
};
