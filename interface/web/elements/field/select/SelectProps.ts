// Copyright © Spatial Corporation. All rights reserved.

import { FieldProps } from "..";

/**
 * Configurable options for a select element.
 */
export type SelectProps = FieldProps & {
  /**
   * An optional placeholder value.
   */
  placeholder?: string;

  /**
   * The select's value.
   */
  value?: string;

  /**
   * An optional change event handler.
   * @param value The new value.
   */
  onChange?: (value: string) => void;
};
