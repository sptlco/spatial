// Copyright © Spatial. All rights reserved.

import { ElementProps } from "../../../..";

/**
 * Configurable options for a menu radio group element.
 */
export type MenuRadioGroupProps = ElementProps & {
  /**
   * The group's current value.
   */
  value?: string;

  /**
   * An optional change event handler.
   * @param value The new value.
   */
  onChange?: (value: string) => void;
};
