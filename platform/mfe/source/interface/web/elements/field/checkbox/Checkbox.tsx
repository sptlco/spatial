// Copyright © Spatial. All rights reserved.

"use client";

import { Checkbox as Primitive } from "@headlessui/react";
import { CheckboxProps, Div, Element, Field, Icon, Label, Node } from "../..";
import clsx from "clsx";

/**
 * Create a new checkbox element.
 * @param props Configurable options for the element.
 * @returns A checkbox element.
 */
export const Checkbox: Element<CheckboxProps> = (
  props: CheckboxProps,
): Node => {
  return (
    <Field
      name={props.name}
      label={props.label}
      description={props.description}
      disabled={props.disabled}
      className={props.className}
    >
      <Field className="gap-1u !flex-row items-start !space-y-0">
        <Primitive
          style={props.style}
          name={props.name}
          checked={props.checked}
          onChange={props.onChange}
          className={clsx(
            "size-3/2u rounded-1/2u group flex cursor-pointer transition-all data-[disabled]:pointer-events-none",
            "bg-background-control-default",
            "ui-checked:bg-background-control-selected-default ui-checked:hover:bg-background-control-selected-hover ui-checked:focus:bg-background-control-selected-focus ui-checked:active:bg-background-control-selected-active",
            "outline-border-control-default hover:outline-border-control-hover focus:outline-border-control-focus active:outline-border-control-active ui-checked:outline-none outline outline-1 focus:outline-2 active:outline-2",
          )}
        >
          <Icon
            className="opacity-0 transition-all group-data-[checked]:opacity-100"
            icon="check"
          />
        </Primitive>
        <Label className="!text-m !font-regular cursor-pointer data-[disabled]:pointer-events-none">
          {props.children}
        </Label>
      </Field>
    </Field>
  );
};
