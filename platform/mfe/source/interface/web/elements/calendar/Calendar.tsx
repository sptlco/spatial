// Copyright © Spatial Corporation. All rights reserved.

"use client";

import { CalendarProps, DateRange, Element, Icon, Node } from "..";
import {
  getDefaultClassNames,
  OnSelectHandler,
  PropsBase,
  DayPicker as UICalendar,
} from "react-day-picker";

import "react-day-picker/style.css";
import { cva } from "class-variance-authority";

const chevronClasses = cva(["ml-1/4u"], {
  variants: {
    orientation: {
      up: "-rotate-90",
      down: "rotate-90",
      left: "rotate-180",
      right: "",
    },
  },
});

/**
 * Create a new calendar element.
 * @param props Configurable options for the element.
 * @returns A calendar element.
 */
export const Calendar: Element<CalendarProps> = ({
  mode = "single",
  numberOfMonths = 1,
  ...props
}: CalendarProps): Node => {
  const defaults = getDefaultClassNames();
  const base: PropsBase = {
    style: props.style,
    captionLayout: "dropdown",
    showOutsideDays: true,
    hideNavigation: true,
    numberOfMonths: numberOfMonths,
    weekStartsOn: 1,
    disabled: props.disabled,
    startMonth: props.startMonth,
    endMonth: props.endMonth,
    defaultMonth: props.defaultMonth,
    components: {
      Chevron: ({ orientation }) => (
        <Icon
          icon="chevron_right"
          className={chevronClasses({ orientation: orientation })}
        />
      ),
    },
    className: props.className,
    classNames: {
      root: `${defaults.root} flex w-fit h-fit`,
      months: `${defaults.months} flex flex-wrap justify-center`,
      month_caption: "text-m font-bold flex items-center justify-center",
      chevron: `${defaults.chevron} !fill-foreground-primary !size-3/2u`,
      weekday: `${defaults.weekday} !text-foreground-secondary !text-s`,
      day: "size-2u transition-all rounded-full hover:bg-background-secondary",
      month_grid: "border-separate border-spacing-1/2u",
      day_button: "size-2u text-s bg-transparent",
      outside: "text-foreground-tertiary",
      disabled: "text-foreground-quaternary",
      today: "bg-background-secondary text-foreground-primary",
      selected: "!bg-background-control-selected-default !text-base-white-9",
      range_start: "",
      range_middle: "!bg-background-secondary !text-foreground-primary",
      range_end: "",
      dropdown: `${defaults.dropdown} !font-bold !text-m !cursor-pointer bg-background-primary text-foreground-primary`,
    },
  };

  switch (mode) {
    case "single":
      return (
        <UICalendar
          {...base}
          mode="single"
          selected={props.selected as Date | undefined}
          onSelect={props.onSelect as OnSelectHandler<Date | undefined>}
        />
      );
    case "range":
      return (
        <UICalendar
          {...base}
          mode="range"
          selected={props.selected as DateRange | undefined}
          onSelect={props.onSelect as OnSelectHandler<DateRange | undefined>}
        />
      );
  }
};
