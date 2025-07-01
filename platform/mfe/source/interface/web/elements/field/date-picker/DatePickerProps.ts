// Copyright © Spatial Corporation. All rights reserved.

import { CalendarMode, DateRange, FieldProps } from "../..";

/**
 * Configurable options for a date picker element.
 */
export type DatePickerProps = FieldProps & {
  /**
   * An optional placeholder value.
   */
  placeholder?: string;

  /**
   * The date picker's current value.
   */
  value?: Date | DateRange;

  /**
   * An optional change event handler.
   * @param value The date picker's current value.
   */
  onChange?: (value: Date | DateRange) => void;

  /**
   * The date picker's mode.
   */
  mode?: CalendarMode;

  /**
   * The earliest month to display.
   */
  startMonth?: Date;

  /**
   * The default month to display.
   */
  defaultMonth?: Date;

  /**
   * The latest month to display.
   */
  endMonth?: Date;
};
