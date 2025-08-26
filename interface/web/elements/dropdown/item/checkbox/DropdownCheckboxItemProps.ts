// Copyright © Spatial Corporation. All rights reserved.

import { ElementProps } from "@spatial/elements";

/**
 * Configurable options for a dropdown checkbox item.
 */
export type DropdownCheckboxItemProps = ElementProps & {
  /**
   * Whether or not the item is checked by default.
   */
  defaultChecked?: boolean;

  /**
   * Whether or not the item is checked.
   */
  checked?: boolean;

  /**
   * An optional change event handler.
   * @param checked Whether or not the item is checked.
   */
  onChange?: (checked: boolean) => void;

  /**
   * Whether or not the item is disabled.
   */
  disabled?: boolean;

  /**
   * An optional shortcut for the item.
   */
  shortcut?: string;
};
