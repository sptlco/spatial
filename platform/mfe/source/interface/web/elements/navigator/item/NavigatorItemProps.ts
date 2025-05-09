// Copyright © Spatial. All rights reserved.

import { ElementProps } from "../..";

/**
 * Configurable options for a navigator item element.
 */
export type NavigatorItemProps = ElementProps & {
  /**
   * Whether or not to render the item as its children.
   */
  asChild?: boolean;

  /**
   * The item's value.
   */
  value?: string;
};
