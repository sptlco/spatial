// Copyright © Spatial Corporation. All rights reserved.

import { FieldProps } from "..";

/**
 * Configurable options for a checkbox element.
 */
export type CheckboxProps = FieldProps & {
  /**
   * Whether or not the checkbox is checked.
   */
  checked?: boolean;

  /**
   * An optional change event handler.
   * @param checked Whether or not the checkbox is checked.
   */
  onChange?: (checked: boolean) => void;
};
