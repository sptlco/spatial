// Copyright © Spatial. All rights reserved.

import { ElementProps } from "../../..";

/**
 * Configurable options for a toggle group item.
 */
export type ToggleGroupItemProps = ElementProps & {
  /**
   * The item's value.
   */
  value: string;

  /**
   * Whether or not the item is disabled.
   */
  disabled?: boolean;
};
