// Copyright © Spatial. All rights reserved.

import { ElementProps } from "../..";

/**
 * Configurable options for a navigator submenu.
 */
export type NavigatorSubmenuProps = ElementProps & {
  /**
   * The submenu's default value.
   */
  defaultValue?: string;

  /**
   * The submenu's value.
   */
  value?: string;

  /**
   * An optional change event handler.
   * @param value The submenu's value.
   */
  onChange?: (value: string) => void;
};
