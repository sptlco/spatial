// Copyright © Spatial Corporation. All rights reserved.

import { Textarea as Primitive } from "@headlessui/react";
import { Element, Field, Node, TextAreaProps } from "../..";
import clsx from "clsx";

/**
 * Create a new text area element.
 * @param props Configurable options for the element.
 * @returns A text area element.
 */
export const TextArea: Element<TextAreaProps> = (
  props: TextAreaProps,
): Node => {
  return (
    <Field
      ref={props.ref}
      style={props.style}
      className={props.className}
      name={props.name}
      label={props.label}
      description={props.description}
      disabled={props.disabled}
    >
      <Primitive
        value={props.value}
        name={props.name}
        onChange={(e) => props.onChange?.(e.target.value)}
        placeholder={props.placeholder}
        readOnly={props.readOnly}
        className={clsx(
          "rounded-1/2u",
          "transition-all",
          "px-1u py-1/2u min-h-8u w-full resize-none",
          "bg-background-control-default outline-border-control-default outline outline-1",
          "placeholder:text-foreground-quaternary",
          "data-[active]:outline-2 data-[focus]:outline-2 group-data-[open]:outline-2",
          "data-[hover]:outline-border-control-hover data-[focus]:!outline-border-control-focus data-[active]:!outline-border-control-active group-data-[open]:!outline-border-control-focus",
          "data-[disabled]:pointer-events-none data-[disabled]:cursor-not-allowed",
        )}
      />
    </Field>
  );
};
