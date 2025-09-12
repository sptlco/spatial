// Copyright © Spatial Corporation. All rights reserved.

import { ElementProps, IconVariant } from "../..";

/**
 * Configurable options for a menu item element.
 */
export type MenuItemProps = ElementProps & {
  /**
   * Whether or not the item is inset.
   */
  inset?: boolean;

  /**
   * An optional icon to render.
   */
  icon?: IconVariant;

  /**
   * An optional shorcut for the item.
   */
  shortcut?: string;

  /**
   * An optional selection event handler.
   * @param event Event context.
   */
  onSelect?: (event: Event) => void;

  /**
   * Whether or not the item is disabled.
   */
  disabled?: boolean;
};
