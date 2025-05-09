// Copyright © Spatial. All rights reserved.

import { ElementProps } from "../..";

/**
 * Configurable options for a popup content element.
 */
export type PopupContentProps = ElementProps & {
  /**
   * Whether or not to render the content as its children.
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
