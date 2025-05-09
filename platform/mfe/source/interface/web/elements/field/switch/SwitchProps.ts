// Copyright © Spatial. All rights reserved.

import { FieldProps } from "..";

/**
 * Configurable options for a switch element.
 */
export type SwitchProps = FieldProps & {
  /**
   * Whether or not the switch is checked.
   */
  checked?: boolean;

  /**
   * An optional change event handler.
   * @param value The new value.
   */
  onChange?: (value: boolean) => void;
};
