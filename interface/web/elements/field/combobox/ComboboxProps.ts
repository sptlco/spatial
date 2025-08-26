// Copyright © Spatial Corporation. All rights reserved.

import { FieldProps, IComboboxOption } from "../..";

/**
 * Configurable options for a combobox element.
 */
export type ComboboxProps = FieldProps & {
  /**
   * An optional placeholder value.
   */
  placeholder?: string;

  /**
   * An optional search placeholder.
   */
  searchPlaceholder?: string;

  /**
   * The combobox's current value.
   */
  value?: string;

  /**
   * An optional change event handler.
   * @param value The combobox's value.
   */
  onChange?: (value: string) => void;

  /**
   * A list of options.
   */
  options: IComboboxOption[];
};
