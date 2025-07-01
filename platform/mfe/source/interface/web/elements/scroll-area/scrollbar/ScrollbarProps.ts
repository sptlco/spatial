// Copyright © Spatial Corporation. All rights reserved.

import { ElementProps } from "../..";

/**
 * Configurable options for a scrollbar element.
 */
export type ScrollbarProps = ElementProps & {
  /**
   * The scrollbar's orientation.
   */
  orientation?: "vertical" | "horizontal";
};
