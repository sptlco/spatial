// Copyright © Spatial Corporation. All rights reserved.

import { ElementProps } from "@spatial/elements";

/**
 * Configurable options for a checklist item element.
 */
export type ChecklistItemProps = ElementProps & {
  /**
   * Whether or not the item is checked.
   */
  checked?: boolean;
};
