// Copyright © Spatial. All rights reserved.

import { ElementProps, IconVariant } from "../../..";

/**
 * Configurable options for a context menu submenu trigger element.
 */
export type ContextMenuSubmenuTriggerProps = ElementProps & {
  /**
   * Whether or not the item is inset.
   */
  inset?: boolean;

  /**
   * An optional icon to render.
   */
  icon?: IconVariant;

  /**
   * Whether or not the trigger is disabled.
   */
  disabled?: boolean;
};
