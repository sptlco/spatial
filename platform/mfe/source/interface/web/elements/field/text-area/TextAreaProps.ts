// Copyright © Spatial Corporation. All rights reserved.

import { FieldProps } from "..";

/**
 * Configurable options for a text area element.
 */
export type TextAreaProps = FieldProps & {
  /**
   * The text area's value.
   */
  value?: string;

  /**
   * An optional change event handler.
   * @param value The text area's value.
   */
  onChange?: (value: string) => void;

  /**
   * An optional placeholder value.
   */
  placeholder?: string;

  /**
   * Whether or not the text area is read-only.
   */
  readOnly?: boolean;
};
