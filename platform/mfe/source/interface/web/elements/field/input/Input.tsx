// Copyright © Spatial. All rights reserved.

"use client";

import { Input as Primitive } from "@headlessui/react";
import { Element, Field, InputProps, Node } from "../..";
import clsx from "clsx";

/**
 * Create a new input element.
 * @param props Configurable options for the element.
 * @returns An input element.
 */
export const Input: Element<InputProps> = ({
  type = "text",
  ...props
}: InputProps): Node => {
  return (
    <Field
      name={props.name}
      label={props.label}
      description={props.description}
      disabled={props.disabled}
      className={clsx(
        { "items-center": props.jumbo },
        props.containerClassName,
      )}
    >
      <Primitive
        ref={props.ref}
        type={type}
        name={props.name}
        value={props.value}
        onChange={(e) => props.onChange?.(e.target.value)}
        minLength={props.minLength}
        maxLength={props.maxLength}
        autoFocus={props.autoFocus}
        readOnly={props.readOnly}
        placeholder={props.placeholder}
        className={clsx(
          "rounded-1/2u",
          "transition-all",
          "px-1u py-1/2u w-full",
          { "!px-0": props.jumbo },
          "overflow-hidden text-ellipsis",
          "placeholder:!text-foreground-quaternary",
          "bg-background-control-default outline-border-control-default outline outline-1",
          "data-[active]:outline-2 data-[focus]:outline-2 group-data-[open]:outline-2",
          {
            "data-[hover]:outline-border-control-hover data-[focus]:!outline-border-control-focus data-[active]:!outline-border-control-active group-data-[open]:!outline-border-control-focus":
              !props.jumbo,
          },
          {
            "!text-center !text-3xl !font-bold !outline-none data-[focus]:!outline-none lg:!text-5xl":
              props.jumbo,
          },
          "data-[disabled]:pointer-events-none data-[disabled]:cursor-not-allowed",
          props.className,
        )}
      />
    </Field>
  );
};
