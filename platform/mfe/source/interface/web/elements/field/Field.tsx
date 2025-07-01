// Copyright © Spatial Corporation. All rights reserved.

import { Field as Primitive } from "@headlessui/react";
import { Description, Element, FieldProps, Label, Node } from "..";
import clsx from "clsx";

/**
 * Create a new field element.
 * @param props Configurable options for the field.
 * @returns A field element.
 */
export const Field: Element<FieldProps> = (props: FieldProps): Node => {
  return (
    <Primitive
      disabled={props.disabled}
      style={props.style}
      className={clsx(
        "w-full",
        "group/field",
        "space-y-1/2u flex flex-col",
        "data-[disabled]:cursor-not-allowed data-[disabled]:opacity-50",
        props.className,
      )}
    >
      {props.label && <Label children={props.label} />}
      {props.children}
      {props.description && <Description children={props.description} />}
    </Primitive>
  );
};
