// Copyright © Spatial. All rights reserved.

import * as Primitive from "@radix-ui/react-select";
import { Element, Field, Icon, Node, SelectProps } from "../..";
import clsx from "clsx";
import { Transition } from "@headlessui/react";
import { Fragment } from "react";

/**
 * Create a new select element.
 * @param props Configurable options for the element.
 * @returns A select element.
 */
export const Select: Element<SelectProps> = (props: SelectProps): Node => {
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
      <Primitive.Root
        name={props.name}
        value={props.value}
        onValueChange={props.onChange}
        disabled={props.disabled}
      >
        <Primitive.Trigger
          className={clsx(
            "group",
            "transition-all",
            "space-x-1u px-1u py-1/2u flex items-center",
            "bg-background-control-default",
            "rounded-1/2u outline-border-control-default outline outline-1",
            "hover:outline-border-control-hover",
            "data-[disabled]:pointer-events-none data-[disabled]:cursor-not-allowed",
            "data-[state=open]:outline-border-control-focus data-[state=open]:outline-2",
            "data-[placeholder]:text-foreground-quaternary",
          )}
        >
          <Primitive.Value placeholder={props.placeholder} />
          <Primitive.Icon className="size-3/2u !ml-auto">
            <Icon
              icon="chevron_right"
              className={clsx(
                "transition-all",
                "text-foreground-primary",
                "group-data-[state=open]:text-border-control-focus",
                "rotate-90 group-data-[state=open]:-rotate-90",
              )}
            />
          </Primitive.Icon>
        </Primitive.Trigger>
        <Primitive.Portal>
          <Primitive.Content
            position="popper"
            sideOffset={8}
            className={clsx(
              "relative",
              "min-w-[var(--radix-select-trigger-width)]",
              "bg-background-primary rounded-1/2u p-1/2u max-h-[var(--radix-select-content-available-height)] overflow-hidden",
              "!outline-border-control-default !outline !outline-1",
              "data-[state=open]:animate-grow",
              "data-[state=closed]:animate-shrink",
            )}
          >
            <Primitive.ScrollUpButton className="flex w-full items-center justify-center">
              <Icon icon="chevron_right" className="-rotate-90" />
            </Primitive.ScrollUpButton>
            <Primitive.Viewport
              className="space-y-1/2u flex flex-col"
              children={props.children}
            />
            <Primitive.ScrollDownButton className="flex w-full items-center justify-center">
              <Icon icon="chevron_right" className="rotate-90" />
            </Primitive.ScrollDownButton>
          </Primitive.Content>
        </Primitive.Portal>
      </Primitive.Root>
    </Field>
  );
};
