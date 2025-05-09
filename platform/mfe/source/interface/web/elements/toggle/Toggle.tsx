// Copyright © Spatial. All rights reserved.

import * as Primitive from "@radix-ui/react-toggle";
import { Element, ElementProps, Node, ToggleProps } from "..";
import clsx from "clsx";

/**
 * Create a new toggle element.
 * @param props Configurable options for the element.
 * @returns A toggle element.
 */
export const Toggle: Element<ToggleProps> = (props: ToggleProps): Node => {
  return (
    <Primitive.Root
      ref={props.ref}
      style={props.style}
      pressed={props.pressed}
      onPressedChange={props.onChange}
      disabled={props.disabled}
      children={props.children}
      className={clsx(
        "rounded-1/2u",
        "transition-all",
        "p-1/2u flex w-fit items-center justify-center",
        "bg-background-interactive-tertiary-default",
        "hover:bg-background-interactive-tertiary-hover",
        "hover:text-foreground-quaternary",
        "data-[state=on]:!bg-background-secondary",
        "data-[state=on]:!text-foreground-primary",
        "data-[disabled]:pointer-events-none",
        "data-[disabled]:cursor-not-allowed",
        "data-[disabled]:opacity-50",
        props.className,
      )}
    />
  );
};
