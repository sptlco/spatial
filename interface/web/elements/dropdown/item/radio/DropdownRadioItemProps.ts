// Copyright © Spatial Corporation. All rights reserved.

import { ElementProps } from "@spatial/elements";

/**
 * Configurable options for a dropdown radio item element.
 */
export type DropdownRadioItemProps = ElementProps & {
  /**
   * The item's value.
   */
  value: string;

  /**
   * An optional selection event handler.
   * @param event Event context.
   */
  onSelect?: (event: Event) => void;

  /**
   * Whether or not the item is disabled.
   */
  disabled?: boolean;

  /**
   * An optional shortcut for the item.
   */
  shortcut?: string;
};
