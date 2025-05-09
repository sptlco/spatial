// Copyright © Spatial. All rights reserved.

import { ElementProps, IconVariant } from "../..";

/**
 * Configurable options for a command item element.
 */
export type CommandItemProps = ElementProps & {
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
   * @param value The value that was selected.
   */
  onSelect?: (value: string) => void;

  /**
   * A hypertextual reference.
   */
  href?: string;
};
