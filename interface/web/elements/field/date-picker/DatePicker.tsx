// Copyright © Spatial Corporation. All rights reserved.

"use client";

import clsx from "clsx";

import {
  Calendar,
  DatePickerProps,
  DateRange,
  Element,
  Field,
  Input,
  Node,
  Popup,
  PopupContent,
  PopupTrigger,
} from "../..";

/**
 * Create a new date picker element.
 * @param props Configurable options for the element.
 * @returns A date picker element.
 */
export const DatePicker: Element<DatePickerProps> = (
  props: DatePickerProps,
): Node => {
  const format = (value?: Date | Date[] | DateRange): string => {
    if (!value) {
      return "";
    }

    if (value instanceof Date) {
      return (value as Date).toLocaleDateString(undefined, {
        month: "2-digit",
        year: "numeric",
        day: "2-digit",
      });
    } else if (Array.isArray(value)) {
      return (value as Date[]).map(format).join(", ");
    } else {
      let range = value as DateRange;
      return `${format(range.from)}${range.to ? " - " + format(range.to) : ""}`;
    }
  };

  return (
    <Field
      label={props.label}
      description={props.description}
      disabled={props.disabled}
      className={props.className}
    >
      <Popup className="group">
        <PopupTrigger className="w-full">
          <Input
            name={props.name}
            value={format(props.value)}
            placeholder={props.placeholder}
          />
        </PopupTrigger>
        <PopupContent className="!py-3/2u flex justify-center">
          <Calendar
            mode={props.mode}
            selected={props.value}
            onSelect={props.onChange}
            startMonth={props.startMonth}
            defaultMonth={props.defaultMonth}
            endMonth={props.endMonth}
          />
        </PopupContent>
      </Popup>
    </Field>
  );
};
