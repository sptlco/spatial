// Copyright © Spatial. All rights reserved.

import { FieldProps } from "../..";

/**
 * Configurable options for an OTP element.
 */
export type OTPProps = FieldProps & {
  /**
   * The maximum length of the OTP.
   */
  maxLength: number;

  /**
   * The field's value.
   */
  value?: string;

  /**
   * An optional change event handler.
   * @param value The field's value.
   */
  onChange?: (value: string) => void;

  /**
   * An optional completion event handler.
   * @param args Field arguments.
   */
  onComplete?: (...args: any[]) => void;
};
