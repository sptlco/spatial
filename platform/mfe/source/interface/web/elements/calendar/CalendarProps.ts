// Copyright © Spatial Corporation. All rights reserved.

import { Matcher, OnSelectHandler } from "react-day-picker";
import { CalendarMode, DateRange, ElementProps } from "..";

/**
 * Configurable options for a calendar element.
 */
export type CalendarProps = ElementProps & {
  /**
   * The calendar's mode.
   */
  mode?: CalendarMode;

  /**
   * The calendar's selected date.
   */
  selected?: Date | DateRange;

  /**
   * An optional callback for when a date is selected.
   */
  onSelect?: OnSelectHandler<Date | DateRange>;

  /**
   * Disabled dates.
   */
  disabled?: Matcher | Matcher[];

  /**
   * The number of months to display.
   */
  numberOfMonths?: number;

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
