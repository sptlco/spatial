// Copyright © Spatial. All rights reserved.

import { ElementProps, IconVariant } from "..";

/**
 * Configurable options for an icon element.
 */
export type IconProps = ElementProps & {
  /**
   * The icon to render.
   */
  icon: IconVariant;
};
