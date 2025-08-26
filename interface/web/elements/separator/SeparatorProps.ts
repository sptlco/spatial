// Copyright © Spatial Corporation. All rights reserved.

import { ElementProps, SeparatorOrientation } from "..";

/**
 * Configurable options for a separator element.
 */
export type SeparatorProps = ElementProps & {
  /**
   * The separator's orientation.
   */
  orientation?: SeparatorOrientation;
};
