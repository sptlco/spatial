// Copyright © Spatial Corporation. All rights reserved.

import { ElementProps } from "../..";

/**
 * Configurable options for a resizeable group element.
 */
export type ResizeableGroupProps = ElementProps & {
  /**
   * The group's direction.
   */
  direction?: "horizontal" | "vertical";
};
