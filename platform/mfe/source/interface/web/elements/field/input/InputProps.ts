// Copyright © Spatial. All rights reserved.

import { FieldProps } from "..";

/**
 * Configurable options for an input element.
 */
export type InputProps = FieldProps & {
  /**
   * The input's type.
   */
  type?: "text" | "password" | "email" | "number";

  /**
   * An optional placeholder value.
   */
  placeholder?: string;

  /**
   * The input's value.
   */
  value?: string;

  /**
   * An optional change event handler.
   * @param value The input's value.
   */
  onChange?: (value: string) => void;

  /**
   * Whether or not the input is read-only.
   */
  readOnly?: boolean;

  /**
   * The minimum length of the field.
   */
  minLength?: number;

  /**
   * The maximum length of the field.
   */
  maxLength?: number;

  /**
   * An optional container class name.
   */
  containerClassName?: string;

  /**
   * Whether or not the input is a jumbo input.
   */
  jumbo?: boolean;
};
