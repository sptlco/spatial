// Copyright © Spatial Corporation. All rights reserved.

import { ElementProps } from "..";

/**
 * Configurable options for a resizeable element.
 */
export type ResizeableProps = ElementProps & {
  /**
   * An optional default size.
   */
  defaultSize?: number;

  /**
   * An optional minimum size.
   */
  minSize?: number;

  /**
   * An optional maximum size.
   */
  maxSize?: number;
};
